using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class StrArrayParamViewModel : ParamViewModelBase
    {
        public StrArrayParamViewModel(StringArrayParameterModel stringArrayParameterModel, bool showPrompt) :
            base(stringArrayParameterModel, showPrompt) 
        {
            //UserInput = string.Join(" ", ((string[])PropertyInfo.GetValue(_propertyOwner)));
        }
    }
}
