using CommunityToolkit.Mvvm.ComponentModel;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class BoolParamViewModel : ParamViewModelBase<bool>
    {
        [ObservableProperty]
        private bool _isUserInput;
        /// <summary>
        /// Notify for boolean change.
        /// </summary>
        /// <param name="value"></param>
        partial void OnIsUserInputChanged(bool value)
        {
            NotifyUserInputChanged();
        }

        /// <summary>
        /// Override to tests the InitialValue against UserInput.
        /// </summary>
        public override bool IsDirty => InitialValue != IsUserInput;

        public BoolParamViewModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : 
            base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            IsUserInput = (bool)PropertyInfo.GetValue(_propertyOwner);
            IsValid = true; // No validation needed for bool
        }

        public override bool TryGetResult(out bool result)
        {
            result = IsUserInput;
            return true;
        }

        public override void Validate() { }
    }
}
    