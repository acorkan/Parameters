using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ParameterModel.Helpers
{
    public static class ImplementParameterAttributeExtension
    {
        private static void UpdateAttributeMap(this IImplementsParameterAttribute implements)
        {
            if (implements.AttributeMap.Count == 0)
            {
                foreach (var attrib in ParameterAttribute.GetAttributeMap(implements))
                {
                    implements.AttributeMap.Add(attrib.Key, attrib.Value);
                }
            }
        }
        public static bool TryResolveVariables(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, string> variableErrors)
        {
            implements.UpdateAttributeMap();
            variableErrors.Clear();
            if ((variablesContext == null) && (implements.VariableAssignments.Count != 0))
            {
                foreach (var variable in implements.VariableAssignments)
                {
                    if (!implements.AttributeMap.ContainsKey(variable.Key))
                    {
                        throw new InvalidOperationException($"Variable '{variable.Key}' not found in attribute map.");
                    }
                    if (variable.Value == null)
                    {
                        variableErrors[variable.Key] = $"Variable assignment '{variable.Key}' is null.";
                        continue;
                    }
                    ParameterAttribute parameterPromptAttribute = implements.AttributeMap[variable.Key];

                    Type type = parameterPromptAttribute.PropertyInfo.PropertyType;
                    if (type == typeof(bool))
                    {
                        if (bool.TryParse(variable.Value, out bool b))
                        {
                            parameterPromptAttribute.PropertyInfo.SetValue(implements, b);
                        }
                        else
                        {
                            variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as a boolean.";
                        }
                    }
                    else if (type == typeof(int))
                    {
                        if (int.TryParse(variable.Value, out int i))
                        {
                            parameterPromptAttribute.PropertyInfo.SetValue(implements, i);
                        }
                        else
                        {
                            variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as an integer.";
                        }
                    }
                    else if (type == typeof(float))
                    {
                        if (float.TryParse(variable.Value, out float f))
                        {
                            parameterPromptAttribute.PropertyInfo.SetValue(implements, f);
                        }
                        else
                        {
                            variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as a float.";
                        }
                    }
                    else if (type == typeof(string))
                    {
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, variable.Value);
                    }
                    else if (type == typeof(string[]))
                    {
                        // Assuming the string array is comma-separated
                        string[] stringArray = variable.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        parameterPromptAttribute.PropertyInfo.SetValue(implements, stringArray);
                    }
                    else if (type.IsEnum)
                    {
                        try
                        {
                            Enum value;
                            string valueString = null;
                            for (int i = 0; i < parameterPromptAttribute.EnumItemsDisplaySource.Count; i++)
                            {
                                if (parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Key.ToString().Equals(valueString, StringComparison.OrdinalIgnoreCase) ||
                                    parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Value.Equals(valueString, StringComparison.OrdinalIgnoreCase))
                                {
                                    value = parameterPromptAttribute.EnumItemsDisplaySource.ElementAt(i).Key;
                                    break;
                                }
                            }
                            if (valueString == null)
                            {
                                variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be resolved for enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                            }
                            else
                            {
                                if (Enum.TryParse(type, valueString, true, out object e))
                                {
                                    parameterPromptAttribute.PropertyInfo.SetValue(implements, e);
                                }
                                else
                                {
                                    variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as enum '{parameterPromptAttribute.PropertyInfo.PropertyType}'.";
                                }
                            }
                        }
                        catch (ArgumentException)
                        {
                            variableErrors[variable.Key] = $"Variable '{variable.Key}' could not be parsed as an enum of type '{type.Name}'.";
                        }
                    }
                    else
                    {
                        throw new NotSupportedException($"Type {type} is not supported.");
                    }
                    variableErrors[variable.Key] = "VariablesContext is null, cannot resolve variables.";
                }
            }
            // This method is intentionally left empty.
            // It can be overridden in derived classes if needed.
            return (variableErrors.Count == 0);
        }

        /// <summary>
        /// Perform a validation on all parameter properties
        /// validateErrors will hold names (Key) of variables that failed to resolve, and their error messages (Value).
        /// </summary>
        /// <param name="validateErrors"></param>
        /// <returns></returns>
        public static bool TryValidateParameters(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, List<string>> validateErrors)
        {
            implements.UpdateAttributeMap();
            validateErrors.Clear();
            Dictionary<string, string> variableErrors = new Dictionary<string, string>();
            bool variablesResolved = implements.TryResolveVariables(variablesContext, variableErrors);

            foreach (var v in implements.AttributeMap)
            {
                if (variableErrors.ContainsKey(v.Key))
                {
                    validateErrors[v.Key] = new List<string> { variableErrors[v.Key] };
                }
                else
                {
                    List<string> errors = new List<string>();
                    if (!implements.ValidateProperty(v.Value.PropertyInfo, errors))
                    {
                        validateErrors[v.Key] = errors;
                    }
                }
            }
            return (validateErrors.Count == 0);
        }

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
                ParameterAttribute? parameterPromptAttribute = allCustomAttributes.OfType<ParameterAttribute>().FirstOrDefault();
                if (parameterPromptAttribute != null)
                {
                    ParameterAttribute.SetPropertyInfo(parameterPromptAttribute, vmProp); // Set the PropertyInfo on the attribute
                    ret.Add(vmProp.Name, parameterPromptAttribute);
                    if (string.IsNullOrEmpty(parameterPromptAttribute.Label))
                    {
                        parameterPromptAttribute.Label = vmProp.Name; // Default label to property name if not set
                    }

                    if (vmProp.PropertyType.IsEnum)
                    {
                        parameterPromptAttribute._enumValues = Enum.GetValues(vmProp.PropertyType);//);.EnumType);
                        if (parameterPromptAttribute._enumValues.Length == 0)
                        {
                            throw new ArgumentException($"Enum type {vmProp.PropertyType} does not contain any values.", nameof(vmProp.Name));
                        }
                        parameterPromptAttribute.EnumItemsDisplaySource =
                            parameterPromptAttribute._enumValues.Cast<Enum>().ToDictionary(s => s, s => EnumToDescriptionOrString(s));
                        //}
                        //_intValues = _enumValues.Cast<int>().ToArray();
                        parameterPromptAttribute._values = parameterPromptAttribute._enumValues.Cast<Enum>().ToDictionary(e => Convert.ToInt32(e), e => EnumToDescriptionOrString(e));
                    }
                    //parameterPromptAttribute._rangeAttribute = allCustomAttributes.OfType<RangeAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._requiredAttribute = allCustomAttributes.OfType<RequiredAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._stringLengthAttribute = allCustomAttributes.OfType<StringLengthAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._emailAddressAttribute = allCustomAttributes.OfType<EmailAddressAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._phoneAttribute = allCustomAttributes.OfType<PhoneAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._urlAttribute = allCustomAttributes.OfType<UrlAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._dataTypeAttribute = allCustomAttributes.OfType<DataTypeAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._enumDataTypeAttribute = allCustomAttributes.OfType<EnumDataTypeAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._fileExtensionsAttribute = allCustomAttributes.OfType<FileExtensionsAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._customValidationAttribute = allCustomAttributes.OfType<CustomValidationAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._displayAttribute = allCustomAttributes.OfType<DisplayAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._displayFormatAttribute = allCustomAttributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
                    //parameterPromptAttribute._editableAttribute = allCustomAttributes.OfType<EditableAttribute>().FirstOrDefault();
                }
            }
            return ret.OrderBy(s => s.Value.PresentationOrder).ToDictionary();
        }


        public static void CopyParameters(IImplementsParameterAttribute source, IImplementsParameterAttribute dest)
        {
            if (source.GetType() != dest.GetType())
            {
                throw new ArgumentException("Source and destination must be of the same type.");
            }
            Dictionary<string, ParameterAttribute> attribMap = ParameterAttribute.GetAttributeMap(source);
            foreach (var attrib in attribMap)
            {
                var sourceValue = attrib.Value.PropertyInfo.GetValue(source);
                attrib.Value.PropertyInfo.SetValue(dest, sourceValue);
            }
        }

        public static void UpdateFromJson(JsonNode jsonNode, IImplementsParameterAttribute dest)
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

    }
}
