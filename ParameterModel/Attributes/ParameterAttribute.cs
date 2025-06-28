using ParameterModel.Interfaces;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ParameterModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ParameterAttribute : Attribute
    {
        /// <summary>
        /// Used in a prompt.
        /// </summary>
        public string Label { get; set; } = "";
        /// <summary>
        /// Optional.
        /// </summary>
        public string Units { get; set; } = "";
        /// <summary>
        /// If localized application, optional. "" value to be ignorred.
        /// </summary>
        public string LanguageKey { get; set; } = "";
        /// <summary>
        /// Use ths to implement an order to how these appear in a dialog or form.
        /// Lower numbers appear first, optional;
        /// </summary>
        public int PresentationOrder { get; set; } = 5;

        /// <summary>
        /// Applies only to numeric inputs.
        /// </summary>
        public float Min { get; set; } = 0.0F;

        /// <summary>
        /// Applies only to numeric inputs.
        /// </summary>
        public float Max { get; set; } = 0.0F;

        public Type EnumType { get; set; } = null!;
        /// <summary>
        /// Optional.
        /// </summary>
        public string ToolTipNotes { get; set; } = "";

        /// <summary>
        /// Applies only to string or string[] inputs
        /// </summary>
        public bool AllowEmptyString { get; set; } = false;

        /// <summary>
        /// If the value can be evaluated as a statement, then this is the type of the evaluation result.
        /// This can only ever be applied to a property type of string.
        /// </summary>
        public Type EvaluateType { get; set; }

        public ParameterAttribute() { }

        public ParameterAttribute(string label)
        { 
            Label = label;
        }

        /// <summary>
        /// Get a map of the PropertyInfo and its corresponding ParameterAttribute.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, ParameterAttribute> GetAttributeMap(IImplementsParameterAttribute obj)
        {
            Dictionary<PropertyInfo, ParameterAttribute> ret = new Dictionary<PropertyInfo, ParameterAttribute>();
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); // | BindingFlags.NonPublic 
            foreach (var vmProp in props)
            {
                ParameterAttribute? parameterPromptAttribute = vmProp.GetCustomAttribute(typeof(ParameterAttribute)) as ParameterAttribute;
                if(parameterPromptAttribute != null)
                {
                    ret.Add(vmProp, parameterPromptAttribute);
                }
            }
            return ret.OrderBy(s => s.Value.PresentationOrder).ToDictionary();
        }

        public static void CopyParameters(IImplementsParameterAttribute source, IImplementsParameterAttribute dest)
        {
            if(source.GetType() != dest.GetType())
            {
                throw new ArgumentException("Source and destination must be of the same type.");
            }
            Dictionary<PropertyInfo, ParameterAttribute> attribMap = GetAttributeMap(source);
            foreach (var prop in attribMap.Keys)
            {
                var sourceValue = prop.GetValue(source);
                prop.SetValue(dest, sourceValue);
            }
        }

        public static void UpdateParametersFromJson(JsonNode jsonNode, IImplementsParameterAttribute dest)
        {
            //if (source.GetType() != dest.GetType())
            //{
            //    throw new ArgumentException("Source and destination must be of the same type.");
            //}
            Dictionary<PropertyInfo, ParameterAttribute> attribMap = GetAttributeMap(dest);
            //foreach (var prop in attribMap.Keys)
            //{
            //    var sourceValue = prop.GetValue(source);
            //    prop.SetValue(dest, sourceValue);
            //}
            foreach (var prop in attribMap.Keys)
            {
                if (prop.CanWrite && jsonNode[prop.Name] != null)
                {
                    var valueNode = jsonNode[prop.Name];

                    try
                    {
                        object? value = valueNode.Deserialize(prop.PropertyType);
                        prop.SetValue(dest, value);
                    }
                    catch
                    {
                        // Optionally log or handle deserialization errors
                    }
                }
            }
        }
    }
}
