using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class StringParamViewModel : ParamViewModelBase
    {
        public StringParamViewModel(StringParameterModel stringParameterModel, bool showPrompt) : 
            base(stringParameterModel, showPrompt) 
        { }
    }
}
