using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System.Reflection;
using System.Text.Json;

namespace ParameterModel.Extensions
{
    public static class ImplementParameterAttributeExtension
    {
        public static bool IsVariableSelected(this IImplementsParameterAttribute implements, string parameterName)
        {
            return implements.ParameterMap[parameterName].IsVariableSelected;
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
        public static bool ResolveVariables(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, string> variableErrors)
        {
            variableErrors.Clear();
            // Nothing to test means nothing to fail.
            if ((implements.ParameterMap == null) || (implements.ParameterMap.Count == 0))
            {
                return true;
            }
            if ((variablesContext != null) && (implements.VariableAssignments.Count != 0))
            {
                List<string> parameterNames = implements.VariableAssignments.Keys.ToList();
                foreach (var propName in parameterNames)
                {
                    string varName = implements.VariableAssignments[propName];
                    // Test if some disconnect with the variables map.
                    if (!implements.ParameterMap.ContainsKey(propName))
                    {
                        throw new InvalidOperationException($"Property '{propName}' not found in attribute map.");
                    }
                    // null variable assignment should not happen
                    if (varName == null)
                    {
                        variableErrors[propName] = $"Variable assignment for '{propName}' is null.";
                        continue;
                    }
                    VariableBase variableBase = variablesContext.GetVariable(varName);
                    if (variableBase == null)
                    {
                        variableErrors[propName] = $"Variable '{varName}' assignment for '{propName}' does not exist.";
                        continue;
                    }
                    IParameterModel parameterPromptAttribute = implements.ParameterMap[propName];
                    Type type = parameterPromptAttribute.ParameterAttribute.PropertyInfo.PropertyType;
                    string assignValue = variableBase.GetValueAsString();
                    if (!parameterPromptAttribute.TestOrSetParameter(assignValue, true))
                    {
                        variableErrors[propName] = $"Parameter '{propName}' of {type} could not be assigned from variable '{varName}' with value '{assignValue}'."; ;
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
        public static bool ValidateParameters(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext, Dictionary<string, List<string>> validateErrors)
        {
            validateErrors.Clear();
            // Nothing to test means nothing to fail.
            if (implements.ParameterMap.Count == 0)
            {
                return true;
            }
            Dictionary<string, string> variableErrors = new Dictionary<string, string>();
            if (variablesContext != null)
            {
                implements.ResolveVariables(variablesContext, variableErrors);
            }
            foreach (var v in implements.ParameterMap)
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
                    if (!implements.ValidateParameter(v.Key, errors))
                    {
                        validateErrors[v.Key] = new List<string>(errors);
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
        private static bool ValidateParameter(this IImplementsParameterAttribute implements,
            string paramName, List<string> errors)
        {
            errors.Clear();
            // Nothing to test means nothing to fail.
            if (implements.ParameterMap.Count == 0)
            {
                return true;
            }
            implements.ParameterMap[paramName].ValidateParameter(errors);
            return errors.Count == 0;
        }

        /// <summary>
        /// Get a map of the PropertyInfo and its corresponding ParameterAttribute.
        /// </summary>
        /// <param name="implements"></param>
        /// <returns></returns>
        public static Dictionary<string, ParameterAttribute> GetParametersMap(this IImplementsParameterAttribute implements)
        {
            Dictionary<string, ParameterAttribute> ret = new Dictionary<string, ParameterAttribute>();
            var props = implements.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); // | BindingFlags.NonPublic 
            foreach (var vmProp in props)
            {
                var allCustomAttributes = vmProp.GetCustomAttributes();
                ParameterAttribute? parameterAttr = allCustomAttributes.OfType<ParameterAttribute>().FirstOrDefault();
                if (parameterAttr != null)
                {
                    List<string> invalidAttributeNames = new List<string>();
                    if (!ParameterAttribute.TestAllowedValidationAttributes(vmProp, invalidAttributeNames))
                    {
                        throw new NotSupportedException($"Parameter named '{vmProp.Name}' of type {vmProp.PropertyType} does not support these attribute(s): " + string.Join(", ", invalidAttributeNames));
                    }

                    ParameterAttribute.SetPropertyInfo(parameterAttr, vmProp, implements); // Set the PropertyInfo on the attribute
                    ret.Add(vmProp.Name, parameterAttr);

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
            foreach (var attrib in source.ParameterMap)
            {
                var sourceValue = attrib.Value.ParameterAttribute.PropertyInfo.GetValue(source);
                attrib.Value.ParameterAttribute.PropertyInfo.SetValue(dest, sourceValue);
            }
        }

        private static readonly string JsonVariablesKey = "__va__";

        public static void UpdateParametersFromJson<T>(this IImplementsParameterAttribute dest, string json)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (dict == null) return;

            // Get all properties.
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Ignore the non-PArameterAttribute properties.
                if (dest.ParameterMap.ContainsKey(prop.Name) && prop.CanWrite && dict.TryGetValue(prop.Name, out var jsonElement))
                {
                    if (prop.PropertyType == typeof(VariableProperty))
                    {
                        // Special case for Variable type, we need to deserialize it differently.
                        var variable = JsonSerializer.Deserialize<VariableProperty>(jsonElement.GetRawText());
                        ((VariableProperty)prop.GetValue(dest)).Assignment = variable.Assignment;
                    }
                    else
                    {
                        object? value = JsonElementToType(jsonElement, prop.PropertyType);
                        prop.SetValue(dest, value);
                    }
                }
            }
            // Handle the special __va__ key for variable assignments.
            if (dict.TryGetValue(JsonVariablesKey, out var varsElement))
            {
                Dictionary<string, string> variableAssignments = JsonSerializer.Deserialize<Dictionary<string, string>>(varsElement.GetRawText());
                dest.VariableAssignments.Clear();
                foreach (var kvp in variableAssignments)
                {
                    dest.VariableAssignments.Add(kvp.Key, kvp.Value);
                }
            }
        }

        private static object? JsonElementToType(JsonElement element, Type type)
        {
            // If the property is a primitive type or string, try direct deserialization
            var raw = element.GetRawText();
            return JsonSerializer.Deserialize(raw, type);
        }

        public static string SerializeParametersToJson(this IImplementsParameterAttribute source, bool includeVariablesAssignment = true)
        {
            var dict = new Dictionary<string, object>();

            foreach (var prop in source.ParameterMap)
            {
                var value = prop.Value.ParameterAttribute.PropertyInfo.GetValue(source);
                dict[prop.Key] = value;
            }
            if (includeVariablesAssignment)
            {
                dict[JsonVariablesKey] = source.VariableAssignments;
            }
            return JsonSerializer.Serialize(dict);
        }

        private static bool TestOrAssignVariable(this IImplementsParameterAttribute implements, string parameterName,
            IVariablesContext variablesContext, string varName, bool setVarValue)
        {
            return implements.GetParameterModel(parameterName).TestOrAssignVariable(variablesContext, varName, setVarValue);
        }

        private static bool TestOrSetParameter(this IImplementsParameterAttribute implements, string paramName,
            string newValue, bool setProperty)
        {
            return implements.ParameterMap[paramName].TestOrSetParameter(newValue, setProperty);
        }

        /// <summary>
        /// Sets the variable value for the specified property name.
        /// Exception if not allowed for property, so be sure CanBeVariable is true.
        /// </summary>
        /// <param name="implements"></param>
        /// <param name="parameterName"></param>
        /// <param name="variableName"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TryAssignVariable(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext,
            string parameterName, string variableName)
        {
            return implements.TestOrAssignVariable(parameterName,
                variablesContext, variableName, true);
        }

        public static bool TrySetParameter(this IImplementsParameterAttribute implements,
            string parameterName, string newValue)
        {
            return implements.GetParameterModel(parameterName).TestOrSetParameter(newValue, true);
        }

        public static bool TestAssignVariable(this IImplementsParameterAttribute implements,
            IVariablesContext variablesContext,
            string parameterName, string variableName)
        {
            return implements.TestOrAssignVariable(parameterName,
                variablesContext, variableName, false);
        }

        public static bool TestSetParameter(this IImplementsParameterAttribute implements,
            string parameterName, string newValue)
        {
            return implements.GetParameterModel(parameterName).TestOrSetParameter(newValue, false);
        }

        public static string GetAssignedVariable(this IImplementsParameterAttribute implements,
            string parameterName)
        {
            return implements.GetParameterModel(parameterName).GetVariableAssignment();
        }

        private static IParameterModel GetParameterModel(this IImplementsParameterAttribute implements,
            string parameterName)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException($"Parameter cannot be null.");
            }
            if (!implements.ParameterMap.ContainsKey(parameterName))
            {
                throw new ArgumentException($"No parameter named '{parameterName}'.");
            }
            return implements.ParameterMap[parameterName];
        }
    }
}

