using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Models.Base;
using System.Collections.ObjectModel;

namespace ParameterViews.ViewModels
{
    public delegate void ParamViewModelChangedDelegate(object sender, string propertyName);

    /// <summary>
    /// Base class for all parameter view models that build off of instances of ParameterPromptAttribute.
    /// </summary>
    public abstract partial class ParamViewModelBase : ViewModelBase<ParameterModelMessage> // ParamViewModelNotifyBase : ViewModelBase<ParameterModelMessage>
    {
        protected ParameterModelBase _model;
        //public event ParamViewModelChangedDelegate OnUserInputChanged;

        /// <summary>
        /// Label for the prompt, typically used in a dialog or form.
        /// </summary>
        public string Prompt { get; }

        public bool ShowPrompt { get; }

        /// <summary>
        /// Tooltip for the prompt.
        /// Same as Prompt but applied to the user input control.
        /// </summary>
        public string PromptToolTip { get; }

        /// <summary>
        /// Echoes a dat annotation that this parameter is read-only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Set if the parameter is read-only, like the dialog is informational and not a prompt.
        /// </summary>
        public bool IsEnabled => !IsReadOnly;

        /// <summary>
        /// Use this to control whgether the user sees a simple edit option or the option to select a variable.
        /// </summary>
        public bool IsVariableOption => _model.ParameterAttribute.CanBeVariable;

        [ObservableProperty]
        private string _errorMsgToolTip;

        [ObservableProperty]
        private bool _isError = false;

        [ObservableProperty]
        private string _userInput;
        /// <summary>
        /// Only notify of a change if the value valid.
        /// Always call Validate() first.
        /// </summary>
        /// <param name="value"></param>
        partial void OnUserInputChanged(string value)
        {
            ApplyUserInputChanged(value);
            //Validate();
            //if (TryGetResult(out T result))
            //{
            //    NotifyUserInputChanged();
            //}
        }

        [ObservableProperty]
        private ObservableCollection<string> _selectionItems;

        [ObservableProperty]
        private string _selectedItem;

        /// <summary>
        /// Try to update the property and then echo what errors result from that
        /// </summary>
        /// <param name="newInput"></param>
        protected void ApplyUserInputChanged(string newInput)
        {
            // If the new value can be used then set it.
            // Does not mean that it passed validations!
            if (_model.ParameterAttribute.TestPropertyValue(newInput, out string errorMessage) &&
                _model.ParameterAttribute.TrySetVariableValue(newInput, out errorMessage))
            {
                //OnUserInputChanged?.Invoke(this, _parameterPromptAttribute.PropertyInfo.Name);
                if(_model.ParameterAttribute.ValidationErrors.Count > 0)
                {
                    // If there are validation errors then set the error message.
                    SetErrorMessage(string.Join(Environment.NewLine, _model.ParameterAttribute.ValidationErrors));
                }
                else
                {
                    // Clear the error message if there are no validation errors.
                    SetErrorMessage(null);
                }
            }
            else
            {
                // If the new value cannot be used then set the error message.
                SetErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// To clear error message, set ErrorMsg to null.
        /// </summary>
        /// <param name="errorMsg"></param>
        protected void SetErrorMessage(string errorMsg)
        {
            ErrorMsgToolTip = errorMsg;
            IsError = !string.IsNullOrEmpty(errorMsg);
        }

        //public abstract bool IsDirty { get; }

        protected ParamViewModelBase(ParameterModelBase model, bool showPrompt)
        {
            _model = model;
            ShowPrompt = ShowPrompt;
            ParameterAttribute parameterAttribute = model.ParameterAttribute;

            PromptToolTip = parameterAttribute.Description;

            string prompt = parameterAttribute.Prompt;
            prompt += ":";
            Prompt = prompt;

            // Get initial value
            if (IsVariableOption)
            {
                SelectionItems = new ObservableCollection<string>(model.GetSelectionItems());
            }
            else 
            {
                UserInput = GetDisplayString(out bool isVariableAssignment);
            }
        }

        protected string GetDisplayString(out bool isVariableAssignment)
        {
            _model.ParameterAttribute.GetDisplayString(out string displayString, out isVariableAssignment);
            return displayString;
        }

        //protected string[] GetItemSelections()
        //{
        //    _parameterPromptAttribute.GetDisplayString .GetDisplayString(out string displayString, out isVariableAssignment);
        //    return displayString;
        //}
    }
}
