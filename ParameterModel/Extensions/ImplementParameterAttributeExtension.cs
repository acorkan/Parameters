using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

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
                    //if (type == typeof(bool))
                    //{
                    //    // If the variable is a boolean, try to parse it.
                    //}
                    //else if (type == typeof(int))
                    //{
                    //    if (int.TryParse(variable.Value, out int i))
                    //    {
                    //        parameterPromptAttribute.PropertyInfo.SetValue(implements, i);
                    //    }
                    //    else
                    //    {
                    //        variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as an integer.";
                    //    }
                    //}
                    //else if (type == typeof(float))
                    //{
                    //    if (float.TryParse(variable.Value, out float f))
                    //    {
                    //        parameterPromptAttribute.PropertyInfo.SetValue(implements, f);
                    //    }
                    //    else
                    //    {
                    //        variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as a float.";
                    //    }
                    //}
                    //else if (type == typeof(string))
                    //{
                    //    parameterPromptAttribute.PropertyInfo.SetValue(implements, variable.Value);
                    //}
                    //else if (type == typeof(string[]))
                    //{
                    //    // Assuming the string array is comma-separated
                    //    string[] stringArray = variable.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //    parameterPromptAttribute.PropertyInfo.SetValue(implements, stringArray);
                    //}
                    //else if (type.IsEnum)
                    //{
                    //    try
                    //    {
                    //        Enum value;
                    //        string valueString = null;
                    //        for (int i = 0; i < parameterPromptAttribute.EnumItemsDisplaySource.Count; i++)
                    //        {
                    //            if (parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Key.ToString().Equals(valueString, StringComparison.OrdinalIgnoreCase) ||
                    //                parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Value.Equals(valueString, StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                value = parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Key;
                    //                break;
                    //            }
                    //        }
                    //        if (valueString == null)
                    //        {
                    //            variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be resolved for enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                    //        }
                    //        else
                    //        {
                    //            if (Enum.TryParse(type, valueString, true, out object e))
                    //            {
                    //                parameterPromptAttribute.PropertyInfo.SetValue(implements, e);
                    //            }
                    //            else
                    //            {
                    //                variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                    //            }
                    //        }
                    //    }
                    //    catch (ArgumentException)
                    //    {
                    //        variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as an enum of type '{type.Name}'.";
                    //    }
                    //}
                    //else
                    //{
                    //    throw new NotSupportedException($"Type {type} is not supported.");
                    //}
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
            Dictionary<string, ParameterAttribute> attribMap = source.GetAttributeMap();
            foreach (var attrib in attribMap)
            {
                var sourceValue = attrib.Value.PropertyInfo.GetValue(source);
                attrib.Value.PropertyInfo.SetValue(dest, sourceValue);
            }
        }

        public static void UpdateFromJson(this IImplementsParameterAttribute dest, JsonNode jsonNode)
        {
            Dictionary<string, ParameterAttribute> attribMap = GetAttributeMap(dest);
            foreach (var prop in attribMap)
            {
                // ?? (prop.CanWrite && 
                if (jsonNode[prop.Key] != null)
                {
                    var valueNode = jsonNode[prop.Key];

                    try
                    {
                        object? value = valueNode.Deserialize(prop.Value.PropertyInfo.PropertyType);
                        prop.Value.PropertyInfo.SetValue(dest, value);
                    }
                    catch
                    {
                        // Optionally log or handle deserialization errors
                    }
                }
            }
        }

        public static string SerializeToJson(IImplementsParameterAttribute source)
        {
            var dict = new Dictionary<string, object>();
            Dictionary<string, ParameterAttribute> attribMap = GetAttributeMap(source);

            foreach (var prop in attribMap)
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

        public static bool TestPropertyValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error)
        {
            return implements.TrySetPropertyValue(propertyName, newValue, out error, false);
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
        private static bool TrySetPropertyValue(this IImplementsParameterAttribute implements,
            string propertyName, string newValue, out string error, bool setProperty)
        {
            error = "";
            if (!implements.AttributeMap.ContainsKey(propertyName))
            {
                throw new ArgumentException($"No property '{propertyName}'.");
            }
            ParameterAttribute parameterPromptAttribute = implements.AttributeMap[propertyName];
            Type type = parameterPromptAttribute.PropertyInfo.PropertyType;
            if (type == typeof(bool))
            {
                if (bool.TryParse(newValue, out bool b))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, b);
                    }
                }
                else
                {
                    error = $"Variable '{propertyName}' could not be parsed as a boolean.";
                }
            }
            else if (type == typeof(int))
            {
                if (int.TryParse(newValue, out int i))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, i);
                    }
                }
                else
                {
                    error = $"Variable '{propertyName}' could not be parsed as an integer.";
                }
            }
            else if (type == typeof(float))
            {
                if (float.TryParse(newValue, out float f))
                {
                    if (setProperty)
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, f);
                    }
                }
                else
                {
                    error = $"Variable '{propertyName}' could not be parsed as a float.";
                }
            }
            else if (type == typeof(string))
            {
                if (setProperty)
                {
                    parameterPromptAttribute.PropertyInfo.SetValue(implements, newValue);
                }
            }
            else if (type == typeof(string[]))
            {
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
                        error = $"Variable '{propertyName}' could not be resolved for enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                    }
                    else
                    {
                        if (Enum.TryParse(type, valueString, true, out object e))
                        {
                            if (setProperty)
                            {
                                parameterPromptAttribute.PropertyInfo.SetValue(implements, e);
                            }
                        }
                        else
                        {
                            error = $"Variable '{propertyName}' could not be parsed as enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                        }
                    }
                }
                catch (ArgumentException)
                {
                    error = $"Variable '{propertyName}' could not be parsed as an enum of type '{type.Name}'.";
                }
            }
            else
            {
                throw new NotSupportedException($"Type {type} is not supported.");
            }
            if (string.IsNullOrEmpty(error))
            {
                //if (!implements.VariableAssignments.ContainsKey(propertyName))
                //{
                //    List<string> errors = new List<string>();
                //    implements.ValidateProperty(parameterPromptAttribute.PropertyInfo, errors);
                //}
                return true;
            }
            return false;
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
            else if (type.IsEnum)
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

