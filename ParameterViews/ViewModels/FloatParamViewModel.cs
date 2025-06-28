using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class FloatParamViewModel : ParamViewModelBase<float>
    {
        // TODO: format string parameter?
        public FloatParamViewModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner, float min, float max) :
            base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            UserInput = ((float)PropertyInfo.GetValue(_propertyOwner)).ToString("F2");
        }

        public override bool TryGetResult(out float result)
        {
            return float.TryParse(UserInput, out result);
        }

        public override void Validate()
        {
            if (!TryGetResult(out float f))
            {
                IsValid = false;
                SetErrorMessage($"Invalid value: {UserInput}");
                return;
            }
            if (_parameterPromptAttribute.Min != _parameterPromptAttribute.Max)
            {
                if(f < _parameterPromptAttribute.Min)
                {
                    IsValid = false;
                    SetErrorMessage($"Value must be greater than or equal to {_parameterPromptAttribute.Min}");
                    return;
                }
                if (f > _parameterPromptAttribute.Max)
                {
                    IsValid = false;
                    SetErrorMessage($"Value must be less than or equal to {_parameterPromptAttribute.Max}");
                    return;
                }
            }
            IsValid = true;
            SetErrorMessage(null);
        }

        public override bool IsDirty
        {
            get
            {
                return IsValid && TryGetResult(out float f) && (f != InitialValue);
            }
        }
    }
}
