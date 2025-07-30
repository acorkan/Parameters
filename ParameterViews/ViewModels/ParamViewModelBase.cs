using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
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
        protected readonly IVariablesContext _variablesContext;
        protected readonly ParameterModelBase _model;
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
        public bool IsVariableOption => _model.ParameterAttribute.CanBeVariable || _model.ParameterType.IsEnum;

        [ObservableProperty]
        private string _errorMsgToolTip;

        [ObservableProperty]
        private bool _isError = false;

        [ObservableProperty]
        private EnumComboBoxItemData _userInput;
        /// <summary>
        /// Only notify of a change if the value valid.
        /// Always call Validate() first.
        /// </summary>
        /// <param name="value"></param>
        partial void OnUserInputChanged(EnumComboBoxItemData value)
        {
            ApplyUserInputChanged(value.Text);
            //Validate();
            //if (TryGetResult(out T result))
            //{
            //    NotifyUserInputChanged();
            //}
        }

        [ObservableProperty]
        private ObservableCollection<EnumComboBoxItemData> _selectionItems;


        /// <summary>
        /// Try to update the property and then echo what errors result from that
        /// </summary>
        /// <param name="newInput"></param>
        protected void ApplyUserInputChanged(string newInput)
        {
            // If the new value can be used then set it.
            // Does not mean that it passed validations!
            bool varAssignOk = false;
            bool paramAssignOk = _model.TestOrSetParameter(newInput, true);
            if (!paramAssignOk && IsVariableOption)
            {
                varAssignOk = _model.TestOrAssignVariable(_variablesContext, newInput, true, out string error);
            }
            if (varAssignOk)
            {
                // Clear errors because as long as the variable is valid for the parameter we are OK.
                SetErrorMessage(null);
            }
            else if (paramAssignOk)
            {
                // A parameter assignment was successful, so we can validate it.
                List<string> errorMessage = new List<string>();
                _model.ValidateParameter(errorMessage);
                if (errorMessage.Count > 0)
                {
                    // If there are validation errors then set the error message.
                    SetErrorMessage(string.Join(Environment.NewLine, errorMessage));
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
                SetErrorMessage($"Input {newInput} is not a valid variable or value.");
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

        protected ParamViewModelBase(ParameterModelBase model, IVariablesContext variablesContext, bool showPrompt)
        {
            _model = model;
            _variablesContext = variablesContext;
            ShowPrompt = showPrompt;
            ParameterAttribute parameterAttribute = model.ParameterAttribute;
            PromptToolTip = parameterAttribute.Description;
            IsReadOnly = _model.IsReadOnly;

            string prompt = parameterAttribute.Prompt;
            prompt += ":";
            Prompt = prompt;

            List<EnumComboBoxItemData> selections =
            [
                .. _model.GetSelectionItems().Select(i => new EnumComboBoxItemData() { Text = i, IsVariable = false }),
                .. _model.GetSelectionVariables(variablesContext).Select(i => new EnumComboBoxItemData() { Text = i, IsVariable = true }),
            ];
            SelectionItems = new ObservableCollection<EnumComboBoxItemData>(selections);

            UserInput = new EnumComboBoxItemData() { Text = GetDisplayString(out bool isVariableAssignment), IsVariable = isVariableAssignment }));
        }

        protected string GetDisplayString(out bool isVariableAssignment)
        {
            string displayString = _model.GetDisplayString(out isVariableAssignment);
            return displayString;
        }

        //protected string[] GetItemSelections()
        //{
        //    _parameterPromptAttribute.GetDisplayString .GetDisplayString(out string displayString, out isVariableAssignment);
        //    return displayString;
        //}
    }
}
