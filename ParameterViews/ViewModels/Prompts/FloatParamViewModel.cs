using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class FloatParamViewModel : ParamViewModelBase
    {
        // TODO: format string parameter?
        public FloatParamViewModel(FloatParameterModel floatParameterModel, IVariablesContext variablesContext, bool showPrompt) :
            base(floatParameterModel, variablesContext, showPrompt)
        {
            IsEditable = true;
        }
    }
}
