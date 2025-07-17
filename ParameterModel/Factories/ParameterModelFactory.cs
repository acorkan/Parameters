using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                IParameterModel parameterModel = null;
                Type type = kvp.Value.PropertyInfo.PropertyType;

                //List<string> invalidAttributeNames = new List<string>();
                //if (!ParameterAttribute.TestAllowedValidationAttributes(kvp.Value.PropertyInfo, invalidAttributeNames))
                //{
                //    throw new NotSupportedException($"Parameter named '{kvp.Value.PropertyInfo.Name}' of type {kvp.Value.PropertyInfo.PropertyType} does not support these attribute(s): " + string.Join(", ", invalidAttributeNames));
                //}
                if (type == typeof(bool))
                {
                    parameterModel = new BoolParameterModel(kvp.Value, _variablesContext);
                }
                else if (type.IsEnum && (type == typeof(Enum)))
                {
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

        //private void ExceptionForInvalidAttribute(Type type, ICollection<Attribute> attributes)
        //{
        //    List<Type> allowed = ParameterAttribute.AllowedValidationAttributes[type];
        //    List<string> attributeNames = new List<string>();
        //    foreach (var attribute in attributes)
        //    {
        //        if(attribute != null)
        //        {
        //            attributeNames.Add(attribute.GetType().Name);
        //        }
        //    }
        //    if (attributeNames.Count != 0)
        //    {
        //        throw new NotSupportedException($"Parameter type {type} does not support these attribute(s): " + string.Join(", ", attributeNames));
        //    }
        //}
    }
}
