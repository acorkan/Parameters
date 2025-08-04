using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class StringParamViewModel : ParamViewModelBase
    {
        public StringParamViewModel(StringParameterModel stringParameterModel, IVariablesContext variablesContext, bool showPrompt) : 
            base(stringParameterModel, variablesContext, showPrompt) 
        {
            IsEditable = true;
        }
    }
}
