using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class IntParamViewModel : ParamViewModelBase
    {
        public IntParamViewModel(IntParameterModel intParameterModel, bool showPrompt) :
            base(intParameterModel, showPrompt)
        {
        }
    }
}
