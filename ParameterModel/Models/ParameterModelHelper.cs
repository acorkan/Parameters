using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterModel.Models
{
    public static class ParameterModelHelper
    {
        public static List<IParameterModel> Collect(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<PropertyInfo, ParameterAttribute> attributeMap = ParameterAttribute.GetAttributeMap(propertyOwner);
            List<IParameterModel> ret = new List<IParameterModel>();
            foreach (var kvp in attributeMap)
            {
                PropertyInfo propertyInfo = kvp.Key;
                ParameterAttribute parameterPromptAttribute = kvp.Value;
                IParameterModel parameterModel = null;
                Type type = null;
                if (parameterPromptAttribute.EvaluateType != null)
                {
                    type = parameterPromptAttribute.EvaluateType;
                }
                else
                {
                    type = propertyInfo.PropertyType;
                }
                if (type == typeof(string))
                {
                    parameterModel = new StringParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }
                else if (type == typeof(int))
                {
                    if (parameterPromptAttribute.EnumType != null)
                    {
                        // If EnumType is specified, use EnumParameterModel
                        parameterModel = new EnumParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                    }
                    else
                    {
                        parameterModel = new IntParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                    }
                }
                else if (type == typeof(float))
                {
                    parameterModel = new FloatParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }
                else if (type == typeof(bool))
                {
                    parameterModel = new BoolParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }
                else if (type == typeof(string[]))
                {
                    parameterModel = new StringArrayParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }
                else
                {
                    throw new NotSupportedException($"Type {type} is not supported.");
                }
                ret.Add(parameterModel);
            }
            return ret;
        }

    }
}
