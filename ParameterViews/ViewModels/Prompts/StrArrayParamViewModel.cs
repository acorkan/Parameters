using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class StrArrayParamViewModel : ParamViewModelBase
    {
        public StrArrayParamViewModel(StringArrayParameterModel stringArrayParameterModel, IVariablesContext variablesContext, bool showPrompt) :
            base(stringArrayParameterModel, variablesContext, showPrompt) 
        {
            //UserInput = string.Join(" ", ((string[])PropertyInfo.GetValue(_propertyOwner)));
        }
    }
}
