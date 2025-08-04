using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class IntParamViewModel : ParamViewModelBase
    {
        public IntParamViewModel(IntParameterModel intParameterModel, IVariablesContext variablesContext, bool showPrompt) :
            base(intParameterModel, variablesContext, showPrompt)
        {
            IsEditable = true;
        }
    }
}
