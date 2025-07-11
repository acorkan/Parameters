using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using System.Reflection;

namespace ParameterModel.Factories
{
    public class ParameterModelFactory
    {
        private IVariablesContext _variablesContext;
        public ParameterModelFactory(IVariablesContext variablesContext) 
        { 
            _variablesContext = variablesContext ?? throw new ArgumentNullException(nameof(variablesContext));
        }

        public Dictionary<string, IParameterModel> Collect(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<string, ParameterAttribute> attributeMap = ParameterAttribute.GetAttributeMap(propertyOwner);
            Dictionary<string, IParameterModel> ret = new Dictionary<string, IParameterModel>();
            foreach (var kvp in attributeMap)
            {
                //PropertyInfo propertyInfo = kvp.Value.PropertyInfo;
                //ParameterAttribute parameterPromptAttribute = kvp.Value;
                IParameterModel parameterModel = null;
                Type type = kvp.Value.PropertyInfo.PropertyType;
                //if (kvp.Value.PropertyInfo.PropertyType != null)
                //{
                //    type = parameterPromptAttribute.VariableType;
                //}
                //else
                //{
                //    type = kvp.Value.PropertyInfo.PropertyType;
                //}
                if (type == typeof(bool))
                {
                    parameterModel = new BoolParameterModel(kvp.Value, kvp.Value.PropertyInfo, propertyOwner, _variablesContext);
                }
/*                else if (type == typeof(string))
                {
                    if (parameterPromptAttribute.EnumType != null)
                    {
                        // If EnumType is specified, use EnumParameterModel
                        parameterModel = new EnumParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner, _evaluationContext);
                    }
                    else
                    {
                        parameterModel = new StringParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                    }
                }
                else if (type == typeof(int))
                {
                    //if (parameterPromptAttribute.EnumType != null)
                    //{
                    //    // If EnumType is specified, use EnumParameterModel
                    //    parameterModel = new EnumParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                    //}
                    //else
                    //{
                        parameterModel = new IntParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                    //}
                }
                else if (type == typeof(float))
                {
                    parameterModel = new FloatParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }
                else if (type == typeof(string[]))
                {
                    parameterModel = new StringArrayParameterModel(parameterPromptAttribute, propertyInfo, propertyOwner);
                }*/
                else
                {
                    throw new NotSupportedException($"Type {type} is not supported.");
                }
                ret.Add(kvp.Value.PropertyInfo.Name, parameterModel);
            }
            return ret;
        }

    }
}
