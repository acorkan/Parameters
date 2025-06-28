using CommunityToolkit.Mvvm.ComponentModel;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.ComponentModel;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    public partial class EnumParamViewModel : ParamViewModelBase<int>
    {
        private Array _enumValues;
        private Array _intValues;

        public string[] EnumItemsSource { get; }

        [ObservableProperty]
        private int _selectedIndex;
        /// <summary>
        /// Notify for combobox change.
        /// </summary>
        /// <param name="value"></param>
        partial void OnSelectedIndexChanged(int value)
        {
            if (TryGetResult(out int result))
            {
                NotifyUserInputChanged();
            }
        }

        public EnumParamViewModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : 
            base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            _enumValues = Enum.GetValues(propertyInfo.PropertyType);
            _intValues = _enumValues.Cast<int>().ToArray();
            EnumItemsSource = _enumValues.Cast<Enum>().Select(s => EnumToDescriptionOrString(s)).ToArray();
            InitialValue = (int)PropertyInfo.GetValue(_propertyOwner);
            SelectedIndex = Array.IndexOf(_intValues, InitialValue);
            IsValid = true; // No validation needed for enum
        }

        private string EnumToDescriptionOrString(Enum value)
        {
            return value.GetType().GetField(value.ToString())
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Cast<DescriptionAttribute>()
                       .FirstOrDefault()?.Description ?? value.ToString();
        }

        public override bool TryGetResult(out int result)
        {
            result = 0;
            if (_intValues != null && _intValues.Length > 0)
            {
                if (SelectedIndex >= 0 && SelectedIndex < _intValues.Length)
                {
                    result = (int)_intValues.GetValue(SelectedIndex);
                    return true;
                }
            }
            return false;
        }

        public override void Validate() { }

        public override bool IsDirty => IsValid && (InitialValue != (int)_intValues.GetValue(SelectedIndex));

    }
}
