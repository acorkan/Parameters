using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class VariableParameterModel : ParameterModelBase
    {
        public VariableParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, variablesContext)
        {
        }
    }
}
