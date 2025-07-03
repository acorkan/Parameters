using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParameterModel.Models
{
    /// <summary>
    /// This wraps the parameter attribute and target object to bind with a ViewModel and perform the attribute validations.
    /// The template T type is validated at runtime.
    /// Also there is a test to see if the value can be evaluated as a statement, if the EvaluateType is set, in which
    /// case the T type must be a string.
    /// Any property with EvaluateType set will never be updated 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterModelBase<T> : ModelBase<ParameterModelMessage>, IParameterModel
    {
        static ParameterModelBase()
        {
            //??TypeValidator.ValidateType<T>();
        }

        public ParameterAttribute ParameterAttribute { get; }
        public PropertyInfo PropertyInfo { get; }
        protected IImplementsParameterAttribute _propertyOwner;
        //protected Func<T, ParameterAttribute, string> _validateFunc;
        protected IStatementEvaluator _statementEvaluator;

        public ParameterModelBase(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner)
        {
            ParameterAttribute = parameterPromptAttribute;
            PropertyInfo = propertyInfo;
            _propertyOwner = propertyOwner;
            //try
            //{
            //    //ValueString = PropertyInfo.GetValue(_propertyOwner).ToString();
            //}
            //catch (Exception ex)
            //{
            //    // If we can't get the value, we will just set it to an empty string.
            //    // This is a fallback to avoid exceptions in the UI.
            //    ValueString = string.Empty;
            //    // Log the exception if needed, or handle it accordingly.
            //    Trace.WriteLine($"Error getting initial value for {PropertyInfo.Name}: {ex.Message}");
            //}
        }


        /// <summary>
        /// Override to allow a string to be parsed as the type T.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract bool TryParse(string valueString, out T value);

        /// <summary>
        /// Convert the type T to a string for display to the user.
        /// Use formatting parameters for this if needed.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract string FormatType(T typeValue);

        /// <summary>
        /// Return the default value.
        /// </summary>
        /// <returns></returns>
        protected abstract T GetDefault();

        /// <summary>
        /// Validate against the attribute settings.
        /// Return null if valid, or a validation error message if invalid.
        /// </summary>
        /// <returns></returns>
        protected abstract string TestAttibuteValidation(T val);

        //protected abstract bool TryGetPropertyValue(string valueString, out T value);

        public bool Validate(List<string> errors)
        {
            errors.Clear();
            if (TryGetValue(out T val, _statementEvaluator, errors))
            {
                string validationError = TestAttibuteValidation(val);
                if (!string.IsNullOrEmpty(validationError))
                {
                    errors.Add(validationError);
                    return false;
                }
                return true;
            }
            errors.Add($"Unable to resolve value of {PropertyInfo.Name} type {PropertyInfo.PropertyType.Name}");
            return false;
        }

        public string Format()
        {
            if (ParameterAttribute.EvaluateType != null)
            {
                return (string)PropertyInfo.GetValue(_propertyOwner);
            }
            else if (ParameterAttribute.EnumType != null) // If the EvaluateType is not set, but EnumType is set, we format the enum value.
            {
                return (string)PropertyInfo.GetValue(_propertyOwner);
            }
            else // Evaluate type is always the property value because it must be a string.
            {
                return FormatType((T)PropertyInfo.GetValue(_propertyOwner));
            }
        }

        //public void SetPropertyValue(T newValue)
        //{
        //    // If the EvaluateType is set, we assume the value is a string and we just set it directly.
        //    if (ParameterAttribute.EvaluateType != null)
        //    {
        //        PropertyInfo.SetValue(_propertyOwner, newValue.ToString());
        //    }
        //    else // If EvaluateType is not set, we assume the value is of type T and we can set it directly.
        //    {
        //        PropertyInfo.SetValue(_propertyOwner, newValue);
        //    }
        //}

        public void SetPropertyValue<V>(V newValue)
        {
            if (newValue is T tValue)
            {
                // If the EvaluateType is set, we assume the value is a string and we just set it directly.
                if (ParameterAttribute.EvaluateType != null)
                {
                    PropertyInfo.SetValue(_propertyOwner, tValue.ToString());
                }
                else // If EvaluateType is not set, we assume the value is of type T and we can set it directly.
                {
                    PropertyInfo.SetValue(_propertyOwner, tValue);
                }
            }
            else
            {
                throw new InvalidCastException($"Cannot cast value of type {typeof(V).Name} to {typeof(T).Name}.");
            }
        }

        public virtual string GetPropertyValueString()
        {
            return PropertyInfo.GetValue(_propertyOwner)?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// This returns selections that can be used in a combobox.
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetSelections();

        //private string _valueString = string.Empty;
        //public string ValueString
        //{
        //    get => _valueString;
        //    set
        //    {
        //        if (_valueString != value)
        //        {
        //            _valueString = value;
        //            SendModelUpdate(nameof(ValueString));
        //        }
        //    }
        //}
        private T _lastParsedValue;
        private bool _lastParsedValueValid;

        /// <summary>
        /// Tries to either parse or evaluate the property value.
        /// For non-evaluation the model provides the value of the property,
        /// but for the evaluation version it will try to evaluate and then parse.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="statementEvaluator"></param>
        /// <returns></returns>
        protected bool TryGetValue(out T value, IStatementEvaluator statementEvaluator = null, List<string> errors = null)
        {
            if (_lastParsedValueValid)
            {
                // If the value has not changed, we can just return the last parsed value.
                value = _lastParsedValue;
                return true;
            }
            value = default;
            //string valueString = GetValueString();

            string valueString = PropertyInfo.GetValue(_propertyOwner)?.ToString() ?? string.Empty;

            // If this is an evaluation then we first try to parse a string for the correct type.
            if (ParameterAttribute.EvaluateType != null) // 
            {
                if (statementEvaluator == null)
                {
                    throw new InvalidOperationException("StatementEvaluator is required for properties with EvaluateType set.");
                }
                if (errors == null)
                {
                    throw new ArgumentNullException(nameof(errors), "Errors list cannot be null.");
                }
                if (statementEvaluator.TryEvaluate(valueString, out string evaluatedValueString, errors))
                {
                    if (TryParse(evaluatedValueString, out value))
                    {
                        _lastParsedValue = value; // Store the last parsed value for quick access next time.
                        _lastParsedValueValid = true;
                    }
                }
            }
            // If this is not an evaluation, or the evaluation failed, we just try to parse the value string directly.
            if (TryParse(valueString, out value))
            {
                _lastParsedValue = value; // Store the last parsed value for quick access next time.
                _lastParsedValueValid = true;
            }
            return _lastParsedValueValid;
        }

        public V GetValue<V>() //IStatementEvaluator statementEvaluator = null, List<string> errors = null)
        {
            List<string> errors = new List<string>();
            if (TryGetValue(out T value, _statementEvaluator, errors))
            {
                if (value is V vValue)
                {
                    return vValue;
                }
                else
                {
                    throw new InvalidCastException($"Cannot cast value of type {typeof(T).Name} to {typeof(V).Name}.");
                }
            }
            return default;
        }

        public override string ToString()
        {
            return $"{PropertyInfo.Name}:{ParameterAttribute.Label}:{PropertyInfo.PropertyType.Name}";
        }


    }
    /*
     *                 string valueString = PropertyInfo.GetValue(_propertyOwner)?.ToString() ?? string.Empty;
            string[] strings = valueString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if((strings.Length == 1) && TryParse(strings[0], out value))
            {
                return true;
            }

     * */

    public class ParameterModelMessage : ModelDependentMessage { }

  
}
