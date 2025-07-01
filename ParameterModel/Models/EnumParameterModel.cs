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
        //private int[] _intValues;
        private readonly Dictionary<int, string> _values;
        private readonly string[] _enumItemsSource;

        public EnumParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
            //if(ParameterAttribute.EvaluateType != null)
            //{
            //    _enumValues = Enum.GetValues(ParameterAttribute.EvaluateType);
            //    _enumItemsSource = _enumValues.Cast<Enum>().Select(s => s.ToString()).ToArray();
            //}
            //else 
            //{
            _enumValues = Enum.GetValues(ParameterAttribute.EnumType);
            if(_enumValues.Length == 0)
            {
                throw new ArgumentException($"Enum type {ParameterAttribute.EnumType} does not contain any values.", nameof(ParameterAttribute.EnumType));
            }
            _enumItemsSource = _enumValues.Cast<Enum>().Select(s => EnumToDescriptionOrString(s)).ToArray();
            //}
            //_intValues = _enumValues.Cast<int>().ToArray();
            _values = _enumValues.Cast<Enum>().ToDictionary(e => Convert.ToInt32(e), e => EnumToDescriptionOrString(e));
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
            return _values.Keys.First(); 
        }

        public override string[] GetSelections()
        {
            return _enumItemsSource;
        }

        protected override string FormatType(int typeValue)
        {
            return _values[typeValue];
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
            return _values.Keys.Contains(val) ? null : $"Value {val} is not a valid selection for {PropertyInfo.Name}.";
        }
    }
}
