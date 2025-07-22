using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class FloatParamViewModel : ParamViewModelBase
    {
        // TODO: format string parameter?
        public FloatParamViewModel(FloatParameterModel floatParameterModel, bool showPrompt) :
            base(floatParameterModel, showPrompt)
        {
        }
    }
}
