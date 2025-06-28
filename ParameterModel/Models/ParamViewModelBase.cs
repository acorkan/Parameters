using CommunityToolkit.Mvvm.ComponentModel;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    /// <summary>
    /// Base class for all parameter view models that build off of instances of ParameterPromptAttribute and have
    /// a template property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class ParamViewModelBase<T> : ParamViewModelNotifyBase
    {
        public T InitialValue { get; protected set; }

        [ObservableProperty]
        private string _userInput;
        /// <summary>
        /// Only notify of a change if the value valid.
        /// Always call Validate() first.
        /// </summary>
        /// <param name="value"></param>
        partial void OnUserInputChanged(string value)
        {
            Validate();
            if (TryGetResult(out T result))
            {
                NotifyUserInputChanged();
            }
        }

        /// <summary>
        /// Write the current value into the property.
        /// </summary>
        /// <returns></returns>
        public override bool TryApplyChangedValue()
        {
            if (TryGetResult(out T result))
            {
                PropertyInfo.SetValue(_propertyOwner, result);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the current value is valid.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public abstract bool TryGetResult(out T result);

        protected ParamViewModelBase(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) :
            base(parameterPromptAttribute, propertyInfo, propertyOwner) 
        {
            InitialValue = (T)PropertyInfo.GetValue(_propertyOwner);
            Validate();
        }
    }
}
