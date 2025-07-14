using ParameterModel.Models;

namespace ParameterViews.ViewModels
{
    public partial class FloatParamViewModel : ParamViewModelBase
    {
        // TODO: format string parameter?
        public FloatParamViewModel(FloatParameterModel floatParameterModel) :
            base(floatParameterModel)
        {
        }
    }
}
