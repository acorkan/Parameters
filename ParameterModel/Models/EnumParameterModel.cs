using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
    public class EnumParameterModel : ParameterModelBase<int>
    {
        private Array _enumValues;
        private Array _intValues;
        private readonly string[] _enumItemsSource;

        public EnumParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            if(ParameterAttribute.EvaluateType != null)
            {
                _enumValues = Enum.GetValues(ParameterAttribute.EvaluateType);
                _enumItemsSource = _enumValues.Cast<Enum>().Select(s => s.ToString()).ToArray();
            }
            else 
            {
                _enumValues = Enum.GetValues(propertyInfo.PropertyType);
                _enumItemsSource = _enumValues.Cast<Enum>().Select(s => EnumToDescriptionOrString(s)).ToArray();
            }
            _intValues = _enumValues.Cast<int>().ToArray();
        }

        private string EnumToDescriptionOrString(Enum value)
        {
            return value.GetType().GetField(value.ToString())
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Cast<DescriptionAttribute>()
                       .FirstOrDefault()?.Description ?? value.ToString();
        }

        protected override int GetDefault()
        {
            return 0; // Default for Enum is not defined, can be set to null or a specific value if needed
        }

        public override string[] GetSelections()
        {
            return _enumItemsSource;
        }

        protected override string FormatType(int typeValue)
        {
            return _enumItemsSource[typeValue];
        }

        protected override bool TryParse(string valueString, out int value)
        {
            value = 0;
            for (int i = 0; i < _enumItemsSource.Length; i++)
            {
                if (_enumItemsSource[i].Equals(valueString, StringComparison.OrdinalIgnoreCase))
                {
                    value = (int)_enumValues.GetValue(i);
                    return true;
                }
            }
            return false;
        }

        protected override string ValidateAttibute(int val)
        {
            if((val < 0) || (val >= _intValues.Length))
            {
                return $"Value {val} is not a valid selection for {PropertyInfo.Name}.";
            }
            return null;
        }
    }
}
