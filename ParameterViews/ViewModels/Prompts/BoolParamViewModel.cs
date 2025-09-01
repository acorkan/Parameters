using CommunityToolkit.Mvvm.ComponentModel;
using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterViews.ViewModels.Prompts
{
    public partial class BoolParamViewModel : ParamViewModelBase
    {
        private bool _intialValue;

        /// <summary>
        /// This is in case we are not implementing a variable option and just want a checkbox.
        /// </summary>
        [ObservableProperty]
        private bool _isUserInput;
        /// <summary>
        /// Notify for boolean change.
        /// </summary>
        /// <param name="value"></param>
        partial void OnIsUserInputChanged(bool value)
        {
            ApplyUserInputChanged(value.ToString());
        }

        public BoolParamViewModel(BoolParameterModel parameterPromptAttribute, IVariablesContext variablesContext, bool showPrompt) : 
            base(parameterPromptAttribute, variablesContext, showPrompt)
        {
            string display = GetDisplayString(out bool isVariableAssignment);
            if (!isVariableAssignment)
            {
                IsUserInput = bool.Parse(display); // Default value if no variable is assigned.
            }
        }
    }
}
    