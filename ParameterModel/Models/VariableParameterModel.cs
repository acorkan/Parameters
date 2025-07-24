using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class VariableParameterModel : ParameterModelBase
    {
        public VariableParameterModel(ParameterAttribute parameterPromptAttribute) : 
            base(parameterPromptAttribute)
        {
        }

        public override VariableType[] AllowedVariableTypes => throw new NotImplementedException();

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplayString()
        {
            throw new NotImplementedException();
        }
    }
}
