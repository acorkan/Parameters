using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public delegate void ParamViewModelChangedDelegate(object sender, string propertyName);

    /// <summary>
    /// Base class for all parameter view models that build off of instances of ParameterPromptAttribute.
    /// </summary>
    public abstract partial class ParamViewModelNotifyBase : ViewModelBase
    {
        protected IImplementsParameterAttribute _propertyOwner;
        protected ParameterAttribute _parameterPromptAttribute;
        public event ParamViewModelChangedDelegate OnUserInputChanged;

        public PropertyInfo PropertyInfo { get; protected set; }

        public string Prompt { get; }

        public string PromptToolTip { get; }

        public bool IsReadOnly { get; set; }

        public bool IsEnabled => !IsReadOnly;

        protected void NotifyUserInputChanged()
        {
            OnUserInputChanged?.Invoke(this, PropertyInfo.Name);
        }

        protected ParamViewModelNotifyBase(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner)
        {
            _parameterPromptAttribute = parameterPromptAttribute;
            PropertyInfo = propertyInfo;
            _propertyOwner = propertyOwner;

            string promptToolTip = null;
            bool doRange = (propertyInfo.PropertyType == typeof(int)) || (propertyInfo.PropertyType == typeof(int));
            if (!string.IsNullOrEmpty(_parameterPromptAttribute.ToolTipNotes))
            {
                promptToolTip = _parameterPromptAttribute.ToolTipNotes;
                if (!promptToolTip.EndsWith("."))
                {
                    promptToolTip += $".";
                }
                if (doRange)
                {
                    string range = (_parameterPromptAttribute.Min == _parameterPromptAttribute.Max) ? 
                        "" : 
                        $" {_parameterPromptAttribute.Min.ToString()} .. {_parameterPromptAttribute.Max.ToString()}";
                    promptToolTip += Environment.NewLine;
                    if (!string.IsNullOrEmpty(range))
                    {
                        promptToolTip += $"Range is{range}";
                    }
                    else
                    {
                        promptToolTip += $"No range limit,";
                    }
                    if (!string.IsNullOrEmpty(_parameterPromptAttribute.Units))
                    {
                        promptToolTip += $" {_parameterPromptAttribute.Units}";
                    }
                    promptToolTip += $".";
                }
            }
            PromptToolTip = promptToolTip;

            string prompt = _parameterPromptAttribute.Label;
            if (!string.IsNullOrEmpty(_parameterPromptAttribute.Units))
            {
                prompt += $" ({_parameterPromptAttribute.Units})";
            }
            prompt += ":";
            Prompt = prompt;
        }

        [ObservableProperty]
        private string _errorMsgToolTip;

        [ObservableProperty]
        private bool _isError = false;

        /// <summary>
        /// To clear error message, set ErrorMsg to null.
        /// </summary>
        /// <param name="errorMsg"></param>
        protected void SetErrorMessage(string errorMsg)
        {
            ErrorMsgToolTip = errorMsg;
            IsError = !string.IsNullOrEmpty(errorMsg);
        }

        /// <summary>
        /// Override this to implement validation of the value.
        /// It should...
        /// 1. Check the value is valid.
        /// 2. Set the IsValid property to true or false.
        /// 3. Set the ErrorMsgToolTip property to a string if there is an error.
        /// </summary>
        /// <returns></returns>
        public abstract void Validate();
        /// <summary>
        /// Set this inside the Validate() method to indicate the value is valid.
        /// </summary>
        public bool IsValid { get; protected set; }

        public abstract bool IsDirty { get; }
        public abstract bool TryApplyChangedValue();
    }
}
