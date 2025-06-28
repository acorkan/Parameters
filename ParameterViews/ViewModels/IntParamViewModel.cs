using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class IntParamViewModel : ParamViewModelBase<int>
    {
        public IntParamViewModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner, int min, int max) :
            base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            UserInput = PropertyInfo.GetValue(_propertyOwner).ToString();
        }

        public override bool TryGetResult(out int result)
        {
            return int.TryParse(UserInput, out result);
        }

        public override void Validate()
        {
            if (!TryGetResult(out int i))
            {
                IsValid = false;
                SetErrorMessage($"Invalid value: {UserInput}");
                return;
            }
            if (_parameterPromptAttribute.Min != _parameterPromptAttribute.Max)
            {
                if (i < (int)_parameterPromptAttribute.Min)
                {
                    IsValid = false;
                    SetErrorMessage($"Value must be greater than or equal to {(int)_parameterPromptAttribute.Min}");
                    return;
                }
                if (i > (int)_parameterPromptAttribute.Max)
                {
                    IsValid = false;
                    SetErrorMessage($"Value must be less than or equal to {(int)_parameterPromptAttribute.Max}");
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
                return IsValid && TryGetResult(out int i) && (i != InitialValue);
            }
        }
    }
}
