using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class StrArrayParamViewModel : ParamViewModelBase<string[]>
    {
        /// <summary>
        /// Assign a lambda to perform custom IsDirtyTest.
        /// </summary>
        public Func<string[], string[], bool> IsDirtyFunc { get; set; }

        /// <summary>
        /// Override to tests the InitialValue against UserInput.
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                string[] userInput = UserInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (IsDirtyFunc != null)
                {
                    return IsDirtyFunc.Invoke(InitialValue, userInput);
                }
                else
                {
                    List<string> initial = new List<string>(InitialValue);
                    if (initial.Count != userInput.Length)
                    {
                        return true;
                    }
                    foreach (string test in userInput)
                    {
                        initial.Remove(test);
                    }
                    return initial.Count != 0;
                }
            }
        }

        public StrArrayParamViewModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) :
            base(parameterPromptAttribute, propertyInfo, propertyOwner) 
        {
            UserInput = string.Join(" ", ((string[])PropertyInfo.GetValue(_propertyOwner)));
        }

        public override bool TryGetResult(out string[] result)
        {
            result = UserInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
