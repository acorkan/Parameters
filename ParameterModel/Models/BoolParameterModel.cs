using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class BoolParameterModel : ParameterModelBase
    {
        public BoolParameterModel(ParameterAttribute parameterPromptAttribute) : 
                base(parameterPromptAttribute)
        {
            _defaultSelections = ["False", "True"];
        }

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if (bool.TryParse(newValue, out bool b))
            {
                if (setProperty)
                {
                    ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, b);
                }
                return true;
            }
            return false;
        }

        protected override string GetDisplayString()
        {
            bool b = (bool)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return b.ToString();
        }
        public override VariableType[] AllowedVariableTypes => [VariableType.Boolean];
    }
}
