using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class StringParamViewModel : ParamViewModelBase<string>
    {
        /// <summary>
        /// Assign a lambda to perform custom IsDirtyTest.
        /// </summary>
        public Func<string, string, bool> IsDirtyFunc { get; set; }

        /// <summary>
        /// Override to tests the InitialValue against UserInput.
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                if (IsDirtyFunc != null)
                {
                    return IsDirtyFunc.Invoke(InitialValue, UserInput);
                }
                return UserInput.Trim() != InitialValue.Trim();
            }
        }

        public StringParamViewModel(ParameterAttribute parameterPromptAttribute, 
                PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : 
            base(parameterPromptAttribute, propertyInfo, propertyOwner) 
        {
            UserInput = PropertyInfo.GetValue(_propertyOwner).ToString();
        }

        public override bool TryGetResult(out string result)
        {
            result = UserInput.Trim();
            return true;
        }

        public override void Validate()
        {
            if (_parameterPromptAttribute.AllowEmptyString || !string.IsNullOrEmpty(UserInput))
            {
                IsValid = true;
                SetErrorMessage(null);
                return;
            }
            IsValid = false;
            SetErrorMessage("Entry cannot be blank");
        }
    }
}
