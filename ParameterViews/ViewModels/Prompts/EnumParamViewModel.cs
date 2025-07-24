using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class EnumParamViewModel : ParamViewModelBase
    {
        public EnumParamViewModel(EnumParameterModel enumParameterModel, IVariablesContext variablesContext, bool showPrompt) : 
            base(enumParameterModel, variablesContext, showPrompt)
        {
        }
    }
}
