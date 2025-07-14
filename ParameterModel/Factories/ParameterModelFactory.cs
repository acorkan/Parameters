using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using System.Reflection;

namespace ParameterModel.Factories
{
    public class ParameterModelFactory
    {
        private readonly IVariablesContext _variablesContext;
        public ParameterModelFactory(IVariablesContext variablesContext) 
        { 
            _variablesContext = variablesContext;
        }

        public Dictionary<string, IParameterModel> GetModels(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<string, ParameterAttribute> attributeMap = propertyOwner.GetAttributeMap();
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
                    parameterModel = new BoolParameterModel(kvp.Value, _variablesContext);
                }
                else if (type.IsEnum && (type == typeof(Enum)))
                {
                    // If EnumType is specified, use EnumParameterModel
                    parameterModel = new EnumParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(string))
                {
                    parameterModel = new StringParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(int))
                {
                    parameterModel = new IntParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(float))
                {
                    parameterModel = new FloatParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(string[]))
                {
                    parameterModel = new StringArrayParameterModel(kvp.Value, _variablesContext);
                }
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
