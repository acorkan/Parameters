using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Models.Base;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class VariableParamViewModel : ParamViewModelBase
    {
        public VariableParamViewModel(VariableParameterModel model, IVariablesContext variablesContext, bool showPrompt) : 
            base(model, variablesContext, showPrompt)
        {
        }
    }
}
