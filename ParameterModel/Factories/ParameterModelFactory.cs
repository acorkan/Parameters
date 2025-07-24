using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Variables;

namespace ParameterModel.Factories
{
    public class ParameterModelFactory : IParameterModelFactory
    {
        public ParameterModelFactory() 
        { 
        }

        public virtual Dictionary<string, IParameterModel> GetModels(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<string, ParameterAttribute> attributeMap = propertyOwner.GetParametersMap();
            Dictionary<string, IParameterModel> ret = new Dictionary<string, IParameterModel>();
            foreach (var kvp in attributeMap)
            {
                IParameterModel parameterModel = null;
                Type type = kvp.Value.PropertyInfo.PropertyType;

                if (type == typeof(bool))
                {
                    parameterModel = new BoolParameterModel(kvp.Value);
                }
                else if (type.IsEnum && (type == typeof(Enum)))
                {
                    parameterModel = new EnumParameterModel(kvp.Value);
                }
                else if (type == typeof(string))
                {
                    parameterModel = new StringParameterModel(kvp.Value);
                }
                else if (type == typeof(Variable))
                {
                    parameterModel = new VariableParameterModel(kvp.Value);
                }
                else if (type == typeof(int))
                {
                    parameterModel = new IntParameterModel(kvp.Value);
                }
                else if (type == typeof(float))
                {
                    parameterModel = new FloatParameterModel(kvp.Value);
                }
                else if (type == typeof(string[]))
                {
                    parameterModel = new StringArrayParameterModel(kvp.Value);
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
