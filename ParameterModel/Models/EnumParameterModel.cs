using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace ParameterModel.Models
{
    public class EnumParameterModel : ParameterModelBase
    {
        private readonly string[] _selections;

        public EnumParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, variablesContext)
        {

            _selections = parameterPromptAttribute.GetEnumItemsDisplay().Values.ToArray();
        }

        public override string[] GetSelectionItems()
        {
            return _selections;
        }
    }
}
