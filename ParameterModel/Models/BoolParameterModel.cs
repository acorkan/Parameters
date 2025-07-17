using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class BoolParameterModel : ParameterModelBase//<bool>
    {
        private readonly string[] _selections = { "True", "False" };

        public BoolParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
                base(parameterPromptAttribute, variablesContext)
        { }

        public override string[] GetSelectionItems()
        {
            return _selections;
        }
    }
}
