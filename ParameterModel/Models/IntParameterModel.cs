using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class IntParameterModel : ParameterModelBase
    {
        public IntParameterModel(ParameterAttribute parameterPromptAttribute) : 
            base(parameterPromptAttribute)
        {
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.Integer];

        protected override string GetDisplayString()
        {
            int i = (int)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return i.ToString();
        }

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if (int.TryParse(newValue, out int i))
            {
                if (setProperty)
                {
                    ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, i);
                }
                return true;
            }
            return false;
        }
    }
}
