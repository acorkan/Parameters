using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class StringParameterModel : ParameterModelBase
    {
        public StringParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext)
            : base(parameterPromptAttribute, variablesContext)
        {
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.String, VariableType.JSON];

        public override bool TestOrSetSetPropertyValue(string newValue, bool setProperty)
        {
            if(newValue == null)
            {
                return false;
            }
            if (setProperty)
            {
                ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, newValue);
            }
            return true;
        }

        protected override string GetDisplayString()
        {
            return ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes).ToString();
        }
    }
}
