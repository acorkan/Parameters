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
    }
}
