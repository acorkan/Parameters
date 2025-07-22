using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class EnumParamViewModel : ParamViewModelBase
    {
        public EnumParamViewModel(EnumParameterModel enumParameterModel, bool showPrompt) : 
            base(enumParameterModel, showPrompt)
        {
        }
    }
}
