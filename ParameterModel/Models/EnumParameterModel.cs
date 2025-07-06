using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
/*    public class EnumParameterModel : ParameterModelBase<Enum>
    {
        private Array _enumValues;
        //private int[] _intValues;
        private readonly Dictionary<int, string> _values;
        private readonly Dictionary<Enum, string> _enumItemsDisplaySource;

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
            _enumItemsDisplaySource = _enumValues.Cast<Enum>().ToDictionary(s => s, s => EnumToDescriptionOrString(s));
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

        protected override Enum GetDefault()
        {
            return _enumItemsDisplaySource.First().Key; 
        }

        public override string[] GetSelectionItems()
        {
            return _enumItemsDisplaySource.Values.ToArray();
        }

        //protected override string FormatType(string typeValue)
        //{
        //    string error = ValidateAttibute(typeValue);
        //    if(!string.IsNullOrEmpty(error))
        //    {
        //        return error;
        //    }
        //    return _values.Values.FirstOrDefault(v => v.Equals(typeValue, StringComparison.OrdinalIgnoreCase)) ?? typeValue;
        //}

        protected override string FormatType(Enum typeValue)
        {
            if(_enumItemsDisplaySource.ContainsKey(typeValue))
            {
                return _enumItemsDisplaySource[typeValue];
            }
            return typeValue.ToString();// _values.Values.FirstOrDefault(v => v.Equals(typeValue, StringComparison.OrdinalIgnoreCase)) ?? typeValue;
        }

        protected override bool TryParse(string valueString, out Enum value)
        {
            value = GetDefault();
            for (int i = 0; i < _enumItemsDisplaySource.Count; i++)
            {
                if (_enumItemsDisplaySource.ElementAt(i).Key.ToString().Equals(valueString, StringComparison.OrdinalIgnoreCase) ||
                    _enumItemsDisplaySource.ElementAt(i).Value.Equals(valueString, StringComparison.OrdinalIgnoreCase))
                {
                    value = _enumItemsDisplaySource.ElementAt(i).Key;
                    return true;
                }
            }
            return false;
        }


        //protected override bool TryParse(string valueString, out string value)
        //{
        //    value = "";
        //    for (int i = 0; i < _enumItemsSource.Length; i++)
        //    {
        //        if (_enumItemsSource[i].Equals(valueString, StringComparison.OrdinalIgnoreCase))
        //        {
        //            value = _enumItemsSource[i];
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Always true for enum, so no return message.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected override string TestAttibuteValidation(Enum val)
        {
            return null;
        }

        //protected override string ValidateAttibute(string val)
        //{
        //    //return _values.Keys.Contains(val) ? null : $"Value {val} is not a valid selection for {PropertyInfo.Name}.";
        //    for (int i = 0; i < _enumItemsDisplaySource.Length; i++)
        //    {
        //        if (_enumItemsDisplaySource[i].Equals(val, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return null;
        //        }
        //    }
        //    return $"Value {val} is not a valid selection for {PropertyInfo.Name}.";
        //}
    }*/
}
