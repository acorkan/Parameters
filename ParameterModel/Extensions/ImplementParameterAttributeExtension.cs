using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParameterModel.Extensions
{
    public static class ImplementParameterAttributeExtension
    {
        private static void UpdateAttributeMap(this IImplementsParameterAttribute implements)
        {
            if (implements.AttributeMap.Count == 0)
            {
                foreach (var attrib in implements.GetAttributeMap())
                {
                    implements.AttributeMap.Add(attrib.Key, attrib.Value);
                }
            }
        }

        public static bool IsVariableSelected(this IImplementsParameterAttribute implements, string propertyName)
        {
            return implements.VariableAssignments.ContainsKey(propertyName);
        }

        /// <summary>
        /// This will take all variables and apply their contents to the associated properties (parameters).
        /// It will return false if any failed and variableErrors will hold the property name (Key) and associated error (value).
        /// Failures will be:
        ///   Missing variable.
        ///   Incorrect variable type, including null.
        ///   Variable map not matching the class properties (not sure how this happens).
        ///   Unsupported parameter property type.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="variablesContext"></param>
        /// <param name="variableErrors"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static bool TryResolveVariables(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, string> variableErrors)
        {
            implements.UpdateAttributeMap();
            variableErrors.Clear();
            if ((variablesContext != null) && (implements.VariableAssignments.Count != 0))
            {
                foreach (var assignment in implements.VariableAssignments)
                {
                    // Test if some disconnect with the variables map.
                    if (!implements.AttributeMap.ContainsKey(assignment.Key))
                    {
                        throw new InvalidOperationException($"Variable '{assignment.Key}' not found in attribute map.");
                    }
                    // null variable assignment should not happen
                    if (assignment.Value == null)
                    {
                        variableErrors[assignment.Key] = $"Variable assignment '{assignment.Key}' is null.";
                        continue;
                    }
                    ParameterAttribute parameterPromptAttribute = implements.AttributeMap[assignment.Key];
                    Type type = parameterPromptAttribute.PropertyInfo.PropertyType;
                    string error = string.Empty;
                    implements.TrySetPropertyValue(assignment.Key, assignment.Value, out error);
                    if(!string.IsNullOrEmpty(error))
                    {
                        variableErrors[assignment.Key] = error;
                    }
                }
            }
            // This method is intentionally left empty.
            // It can be overridden in derived classes if needed.
            return (variableErrors.Count == 0);
        }

        /// <summary>
        /// Perform a validation on all parameter properties and return false if any errors.
        /// If errors then validateErrors will hold names (Key) of parameters that failed to resolve, and their error messages (Value).
        /// If variablesContext is null then any properties with variable assingments are ignorred, but if not null then TryResolveVariables()
        /// will be called and if no variable error for that property, then it will be evaluated.
        /// </summary>
        /// <param name="validateErrors"></param>
        /// <returns></returns>
        public static bool TryValidateParameters(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, List<string>> validateErrors)
        {
            implements.UpdateAttributeMap();
            validateErrors.Clear();
            Dictionary<string, string> variableErrors = new Dictionary<string, string>();
            if (variablesContext != null)
            {
                implements.TryResolveVariables(variablesContext, variableErrors);
            }

            foreach (var v in implements.AttributeMap)
            {
                // If we resolved variables and this variable has an error, then skip validation
                // for it but add to error list.
                if ((variablesContext != null) && variableErrors.ContainsKey(v.Key))
                {
                    validateErrors[v.Key] = new List<string> { variableErrors[v.Key] };
                }
                else // If no variable errors then validate the property.
                {
                    List<string> errors = new List<string>();
                    if (!implements.ValidateProperty(v.Value.PropertyInfo, errors))
                    {
                        validateErrors[v.Key].AddRange(errors);
                    }
                }
            }
            return (validateErrors.Count == 0);
        }

        /// <summary>
        /// Validate a specific property.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool ValidateProperty(this IImplementsParameterAttribute implements,
            PropertyInfo propertyInfo, List<string> errors)
        {
            implements.UpdateAttributeMap();
            errors.Clear();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(implements) { MemberName = propertyInfo.Name };
            object value = propertyInfo.GetValue(implements);
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return !errors.Any();
        }

        /// <summary>
        /// Get a map of the PropertyInfo and its corresponding ParameterAttribute.
        /// </summary>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static Dictionary<string, ParameterAttribute> GetAttributeMap(this IImplementsParameterAttribute implements)
        {
            Dictionary<string, ParameterAttribute> ret = new Dictionary<string, ParameterAttribute>();
            var props = implements.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); // | BindingFlags.NonPublic 
            foreach (var vmProp in props)
            {
                var allCustomAttributes = vmProp.GetCustomAttributes();
                //ParameterAttribute? parameterPromptAttribute = vmProp.GetCustomAttribute(typeof(ParameterAttribute)) as ParameterAttribute;
                ParameterAttribute? parameterAttr = allCustomAttributes.OfType<ParameterAttribute>().FirstOrDefault();
                if (parameterAttr != null)
                {
                    ParameterAttribute.SetPropertyInfo(parameterAttr, vmProp, implements); // Set the PropertyInfo on the attribute
                    ret.Add(vmProp.Name, parameterAttr);
                    if (string.IsNullOrEmpty(parameterAttr.Label))
                    {
                        parameterAttr.Label = vmProp.Name; // Default label to property name if not set
                    }

                    if (vmProp.PropertyType.IsEnum)
                    {
                        ParameterAttribute.InitEnumData(parameterAttr);
                    }
                }
            }
            return ret.OrderBy(s => s.Value.PresentationOrder).ToDictionary();
        }


        public static void CopyParameters(this IImplementsParameterAttribute source, IImplementsParameterAttribute dest)
        {
            if (source.GetType() != dest.GetType())
            {
                throw new ArgumentException("Source and destination must be of the same type.");
            }
            source.UpdateAttributeMap();
            dest.UpdateAttributeMap();
            //Dictionary<string, ParameterAttribute> attribMap = source.GetAttributeMap();
            foreach (var attrib in source.AttributeMap)
            {
                var sourceValue = attrib.Value.PropertyInfo.GetValue(source);
                attrib.Value.PropertyInfo.SetValue(dest, sourceValue);
            }
        }

        public static void UpdateFromJson<T>(this IImplementsParameterAttribute dest, string json)
        {
            dest.UpdateAttributeMap();
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (dict == null) return;

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (dest.AttributeMap.ContainsKey(prop.Name) && prop.CanWrite && dict.TryGetValue(prop.Name, out var jsonElement))
                {
                    try
                    {
                        object? value = JsonElementToType(jsonElement, prop.PropertyType);
                        prop.SetValue(dest, value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to set property {prop.Name}: {ex.Message}");
                    }
                }
            }
        }

        private static object? JsonElementToType(JsonElement element, Type type)
        {
            // If the property is a primitive type or string, try direct deserialization
            var raw = element.GetRawText();
            return JsonSerializer.Deserialize(raw, type);
        }

        public static string SerializeToJson(this IImplementsParameterAttribute source)
        {
            source.UpdateAttributeMap();
            var dict = new Dictionary<string, object>();

            foreach (var prop in source.AttributeMap)
            {
                var value = prop.Value.PropertyInfo.GetValue(source);
                dict[prop.Key] = value;
            }
            return JsonSerializer.Serialize(dict);
        }

        //private static string _varRegexPattern = @"^[A-Za-z_][A-Za-z0-9_]*$";

        //private static System.Text.RegularExpressions.Regex _varNameRegex = new System.Text.RegularExpressions.Regex(_varRegexPattern);

        public static bool TrySetPropertyValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error)
        {
            return implements.TrySetPropertyValue(propertyName, newValue, out error, true);
        }

        public static bool TrySetVariableValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error)
        {
            error = "";
            try
            {
                implements.SetVariableValue(propertyName, newValue);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool TestPropertyValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error)
        {
            return implements.TrySetPropertyValue(propertyName, newValue, out error, false);
        }

        /// <summary>
        /// Sets the variable value for the specified property name.
        /// Exception if not allowed for property, so be sure CanBeVariable is true.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetVariableValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue)
        {
            implements.UpdateAttributeMap();
            if (!implements.AttributeMap.ContainsKey(propertyName))
            {
                throw new ArgumentException($"No property '{propertyName}'.");
            }
            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentNullException(nameof(newValue), "New variable name cannot be null or empty.");
            }
            ParameterAttribute parameterPromptAttribute = implements.AttributeMap[propertyName];
            if(parameterPromptAttribute.CanBeVariable == false)
            {
                throw new InvalidOperationException($"Property '{propertyName}' is not marked as a variable assignment.");
            }
            if (parameterPromptAttribute.IsReadOnly)
            {
                throw new InvalidOperationException($"Parameter {propertyName} is read-only.");
            }
            // OK to assign variable.
            implements.VariableAssignments[propertyName] = newValue;
            // Clear property errors.
            parameterPromptAttribute.ValidationErrors.Clear();
        }

        /// <summary>
        /// Apply the user supplied string to the parameter property and clear the variable assignment if any.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="setVariable"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static bool TrySetPropertyValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error, bool setProperty)
        {
            implements.UpdateAttributeMap();
            error = "";
            if (!implements.AttributeMap.ContainsKey(propertyName))
            {
                throw new ArgumentException($"No property '{propertyName}'.");
            }
            ParameterAttribute parameterPromptAttribute = implements.AttributeMap[propertyName];
            if(parameterPromptAttribute.IsReadOnly)
            {
                error = $"Parameter {propertyName} is read only";
                return false;
            }
            Type type = parameterPromptAttribute.PropertyInfo.PropertyType;
            bool parsedOK = false;
            if (type == typeof(bool))
            {
                if (parsedOK = bool.TryParse(newValue, out bool b))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, b);
                    }
                }
            }
            else if (type == typeof(int))
            {
                if (parsedOK = int.TryParse(newValue, out int i))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, i);
                    }
                }
            }
            else if (type == typeof(float))
            {
                if (parsedOK = float.TryParse(newValue, out float f))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, f);
                    }
                }
            }
            else if (type == typeof(string))
            {
                parsedOK = true;
                if (setProperty)
                {
                    parameterPromptAttribute.PropertyInfo.SetValue(implements, newValue);
                }
            }
            else if (type == typeof(string[]))
            {
                parsedOK = true;
                // Assuming the string array is comma-separated
                if (setProperty)
                {
                    string[] stringArray = newValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    parameterPromptAttribute.PropertyInfo.SetValue(implements, stringArray);
                }
            }
            else if (type.IsEnum)
            {
                try
                {
                    Enum value;
                    string valueString = null;
                    for (int i = 0; i < parameterPromptAttribute.EnumItemsDisplayDict.Count; i++)
                    {
                        if (parameterPromptAttribute.EnumItemsDisplayDict.ElementAt(i).Key.ToString().Equals(valueString, StringComparison.OrdinalIgnoreCase) ||
                            parameterPromptAttribute.EnumItemsDisplayDict.ElementAt(i).Value.Equals(valueString, StringComparison.OrdinalIgnoreCase))
                        {
                            value = parameterPromptAttribute.EnumItemsDisplayDict.ElementAt(i).Key;
                            break;
                        }
                    }
                    if (valueString == null)
                    {
                        error = $"Parameter '{propertyName}' could not be resolved for enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                    }
                    else
                    {
                        if (Enum.TryParse(type, valueString, true, out object e))
                        {
                            if (setProperty)
                            {
                                parameterPromptAttribute.PropertyInfo.SetValue(implements, e);
                            }
                            parsedOK = true;
                        }
                        else
                        {
                            error = $"Parameter '{propertyName}' could not be parsed as enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                        }
                    }
                }
                catch (ArgumentException)
                {
                    error = $"Parameter '{propertyName}' could not be parsed as an enum of type '{type.Name}'.";
                }
            }
            else
            {
                throw new NotSupportedException($"Type {type} is not supported.");
            }
            // Update the error message if parsing failed and not already updated.
            if (!parsedOK && string.IsNullOrEmpty(error))
            {
                error = $"Parameter '{propertyName}' of {type} could not be assigned from '{newValue}'.";
            }
            else if (parsedOK && setProperty)
            {
                if (implements.VariableAssignments.ContainsKey(propertyName))
                {
                    implements.VariableAssignments.Remove(propertyName);
                }
                implements.ValidateProperty(parameterPromptAttribute.PropertyInfo, parameterPromptAttribute.ValidationErrors);
            }
            return parsedOK;
        }

        /// <summary>
        /// Apply the user supplied string to the variable or the variable name.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="setVariable"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool GetDisplayString(this IImplementsParameterAttribute implements,
            string propertyName, out string displayString, out bool isVariableAssignment)
        {
            displayString = "";
            isVariableAssignment = false;
            if (!implements.AttributeMap.ContainsKey(propertyName))
            {
                throw new ArgumentException($"No property '{propertyName}'.");
            }
            ParameterAttribute parameterPromptAttribute = implements.AttributeMap[propertyName];
            if(implements.VariableAssignments.ContainsKey(propertyName))
            {
                // If the variable is set, then return that value.
                displayString = implements.VariableAssignments[propertyName];
                isVariableAssignment = true;
                return true;
            }
            Type type = parameterPromptAttribute.PropertyInfo.PropertyType;
            if (type == typeof(bool))
            {
                if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is bool b)
                {
                    displayString = b.ToString();
                    return true;
                }
            }
            else if (type == typeof(int))
            {
                if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is int i)
                {
                    displayString = i.ToString();
                    return true;
                }
            }
            else if (type == typeof(float))
            {
                if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is float f)
                {
                    displayString = f.ToString();
                    return true;
                }
            }
            else if (type == typeof(string))
            {
                if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is string s)
                {
                    displayString = s;
                    return true;
                }
            }
            else if (type == typeof(string[]))
            {
                if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is string[] stringArray)
                {
                    displayString = string.Join(" ", stringArray);
                    return true;
                }
            }
            else if (type.IsEnum && (type == typeof(Enum)))
            {

                try
                {
                    if (parameterPromptAttribute.PropertyInfo.GetValue(implements) is Enum typeValue)
                    {
                        if (parameterPromptAttribute.EnumItemsDisplayDict.ContainsKey(typeValue))
                        {
                            displayString = parameterPromptAttribute.EnumItemsDisplayDict[typeValue];
                        }
                        else
                        {
                            displayString = typeValue.ToString();// _values.Values.FirstOrDefault(v => v.Equals(typeValue, StringComparison.OrdinalIgnoreCase)) ?? typeValue;
                        }
                        return true;
                    }
                }
                catch (ArgumentException)
                {
                    displayString = $"Variable '{propertyName}' could not be parsed as an enum of type '{type.Name}'.";
                }
            }
            else
            {
                throw new NotSupportedException($"Type {type} is not supported.");
            }
            return false;
        }
    }
}

