using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class BoolParameterModel : ParameterModelBase
    {
        private readonly string[] _selections = { "False", "True" };

        public BoolParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
                base(parameterPromptAttribute, variablesContext)
        { }

        public override string[] GetSelectionItems()
        {
            return _selections;
        }

        public override bool TestOrSetSetPropertyValue(string newValue, bool setProperty)
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
