using ParameterModel.Models;

namespace ParameterViews.ViewModels
{
    public partial class StrArrayParamViewModel : ParamViewModelBase
    {
        public StrArrayParamViewModel(StringArrayParameterModel stringArrayParameterModel) :
            base(stringArrayParameterModel) 
        {
            //UserInput = string.Join(" ", ((string[])PropertyInfo.GetValue(_propertyOwner)));
        }
    }
}
