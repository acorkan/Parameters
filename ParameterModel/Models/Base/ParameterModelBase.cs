using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParameterModel.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterModelBase<T> : ModelBase<ParameterModelMessage>, IParameterModel<T>
    {
        static ParameterModelBase()
        {
            //??TypeValidator.ValidateType<T>();
        }

        protected IImplementsParameterAttribute _propertyOwner;
        protected IStatementEvaluator _statementEvaluator;

        public IEvaluationContext EvaluationContext { get; }

        public ParameterModelBase(ParameterAttribute parameterPromptAttribute, 
            PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner,
            IEvaluationContext evaluationContext)
        {
            ParameterAttribute = parameterPromptAttribute;
            PropertyInfo = propertyInfo;
            _propertyOwner = propertyOwner;
            IsPropertyTypeString = PropertyInfo.PropertyType == typeof(string);
            CanEvaluate = ParameterAttribute.EvaluateType != null;
            EvaluationContext = evaluationContext;
            if (CanEvaluate && (EvaluationContext == null))
            {
                throw new InvalidOperationException($"EvaluationContext cannot be null for properties like {PropertyInfo.Name} with EvaluateType set.");
            }

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
        //protected abstract bool TryParse(string valueString, out T value, out string parseError);

        protected virtual bool TryParse(string valueString, out T value, out string parseError)
        {
            parseError = "";
            value = GetDefault();
            if (valueString == null)
            {
                parseError = "Value string cannot be null.";
                return false;
            }
            if (TryParse(valueString, out value))
            {
                return true;
            }
            else
            {
                parseError = $"Failed to parse '{valueString}' as a typeof(T) value.";
                return false;
            }
        }

        #region IParameterModel
        public ParameterAttribute ParameterAttribute { get; }
        public bool IsPropertyTypeString { get; }
        public PropertyInfo PropertyInfo { get; }

        public bool CanEvaluate { get; }

        public string AttributeError { get; protected set; }

        public bool IsAttributeError { get => !string.IsNullOrEmpty(AttributeError); }

        public string ParameterError { get; set; }

        public bool IsParameterError { get => !string.IsNullOrEmpty(ParameterError);  }

        public bool IsEvaluateError { get; protected set; }

        public bool IsAnyError { get => IsAttributeError || IsParameterError || IsEvaluateError; }

        public virtual string ToDisplayString()
        {
            if (IsPropertyTypeString) // && PropertyInfo.GetValue(_propertyOwner) is string val)
            {
                return (string)PropertyInfo.GetValue(_propertyOwner) ?? string.Empty;
            }
            else
            {
                T val = (T)PropertyInfo.GetValue(_propertyOwner);
                return ToDisplayString(val);
            }
        }

        public bool TestValueString(string valueString, out string parseError)
        {
            return TryParse(valueString, out T foo, out parseError);
        }

        public void SetPropertyValueString(string newValueString)
        {
            if (IsPropertyTypeString)
            {
                PropertyInfo.SetValue(_propertyOwner, newValueString);
            }
            throw new ArgumentException($"Invalid value for {PropertyInfo.Name} type, it is not a string.");
        }

        public string GetPropertyValueString()
        {
            if (IsPropertyTypeString)
            {
                return (string)PropertyInfo.GetValue(_propertyOwner);
            }
            throw new ArgumentException($"{PropertyInfo.Name} type, it is not a string.");
        }


        /// <summary>
        /// This returns selections that can be used in a combobox.
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetSelectionItems();

        #endregion IParameterModel
        #region IParameterModel<T>
        /// <summary>
        /// Validate the specified value against the attribute settings.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="attributeError"></param>
        /// <returns></returns>
        public abstract bool TestAttibuteValidation(T val, out string attributeError);


        /// <summary>
        /// Return the default value.
        /// </summary>
        /// <returns></returns>
        public abstract T GetDefault();

        //public abstract bool TryGetFormat(T val, out string formattedValue);

        public abstract string ToDisplayString(T val);

        public void SetPropertyValue(T newValue)
        {
            if (IsPropertyTypeString)// && (typeof(T) != typeof(string)))
            {
                PropertyInfo.SetValue(_propertyOwner, ToDisplayString(newValue));
            }
            else
            {
                PropertyInfo.SetValue(_propertyOwner, newValue);
            }
        }

        public bool TryGetPropertyValue(out T val, out string propertyError)
        {
            propertyError = "";
            val = GetDefault();
            AttributeError = null;
            if (IsPropertyTypeString)
            {
                string propertyString = (string)PropertyInfo.GetValue(_propertyOwner);

                if (string.IsNullOrEmpty(propertyString))
                {
                    return false; // No value to parse.
                }
                if (_statementEvaluator != null)
                {
                    List<string> errors = new List<string>();
                    if (_statementEvaluator.TryEvaluate(propertyString, out string result, errors))
                    {
                        propertyString = result;
                    }
                    else
                    {
                        propertyError = string.Join(", ", errors); // Collect all errors from evaluation.
                        AttributeError = propertyError;
                        return false; // Evaluation failed, no value to parse.
                    }
                }

                if (TryParse(propertyString, out val, out propertyError))
                {
                    return true; // Successfully parsed the value.
                }
                else
                {
                    AttributeError = propertyError;
                }
            }
            else
            {
                val = (T)PropertyInfo.GetValue(_propertyOwner);
                return true; // Parsing failed.
            }
            return false;
        }

        public abstract bool TryParse(string valString, out T val);
        #endregion IParameterModel<T>

        //public abstract bool TryGetValue(out T val);


        //public abstract bool TestValue(T val, out string parseError);

        //public bool TestValue<T>(T val, out string parseError)
        //{
        //    throw new NotImplementedException();
        //}

        //public T GetDefault<T>()
        //{
        //    throw new NotImplementedException();
        //}

        //public T1 GetValue<T1>()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool SetPropertyValueFromString(string newValue)
        //{
        //    if (TryParse(newValue, out T tValue, out string err) && TestValue(tValue, out err))
        //    {
        //        PropertyInfo.SetValue(_propertyOwner, tValue);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool SetPropertyValue(T newValue)
        //{
        //    if (TestValue(newValue, out string err))
        //    {
        //        if (PropertyTypeIsString)
        //        {
        //            PropertyInfo.SetValue(_propertyOwner, newValue.ToString());
        //        }
        //        else
        //        {
        //            PropertyInfo.SetValue(_propertyOwner, newValue);
        //        }
        //        return true;
        //    }
        //    return false;
        //}


        ////private string _valueString = string.Empty;
        ////public string ValueString
        ////{
        ////    get => _valueString;
        ////    set
        ////    {
        ////        if (_valueString != value)
        ////        {
        ////            _valueString = value;
        ////            SendModelUpdate(nameof(ValueString));
        ////        }
        ////    }
        ////}
        //private T _lastParsedValue;
        //private bool _lastParsedValueValid;

        ///// <summary>
        ///// Tries to either parse or evaluate the property value.
        ///// For non-evaluation the model provides the value of the property,
        ///// but for the evaluation version it will try to evaluate and then parse.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="statementEvaluator"></param>
        ///// <returns></returns>
        //protected bool TryGetValue(out T value, IStatementEvaluator statementEvaluator = null, List<string> errors = null)
        //{
        //    if (_lastParsedValueValid)
        //    {
        //        // If the value has not changed, we can just return the last parsed value.
        //        value = _lastParsedValue;
        //        return true;
        //    }
        //    value = default;
        //    //string valueString = GetValueString();

        //    string valueString = PropertyInfo.GetValue(_propertyOwner)?.ToString() ?? string.Empty;

        //    // If this is an evaluation then we first try to parse a string for the correct type.
        //    if (ParameterAttribute.EvaluateType != null) // 
        //    {
        //        if (statementEvaluator == null)
        //        {
        //            throw new InvalidOperationException("StatementEvaluator is required for properties with EvaluateType set.");
        //        }
        //        if (errors == null)
        //        {
        //            throw new ArgumentNullException(nameof(errors), "Errors list cannot be null.");
        //        }
        //        if (statementEvaluator.TryEvaluate(valueString, out string evaluatedValueString, errors))
        //        {
        //            if (TryParse(evaluatedValueString, out value))
        //            {
        //                _lastParsedValue = value; // Store the last parsed value for quick access next time.
        //                _lastParsedValueValid = true;
        //            }
        //        }
        //    }
        //    // If this is not an evaluation, or the evaluation failed, we just try to parse the value string directly.
        //    if (TryParse(valueString, out value))
        //    {
        //        _lastParsedValue = value; // Store the last parsed value for quick access next time.
        //        _lastParsedValueValid = true;
        //    }
        //    return _lastParsedValueValid;
        //}

        //public V GetValue<V>() //IStatementEvaluator statementEvaluator = null, List<string> errors = null)
        //{
        //    List<string> errors = new List<string>();
        //    if (TryGetValue(out T value, _statementEvaluator, errors))
        //    {
        //        if (value is V vValue)
        //        {
        //            return vValue;
        //        }
        //        else
        //        {
        //            throw new InvalidCastException($"Cannot cast value of type {typeof(T).Name} to {typeof(V).Name}.");
        //        }
        //    }
        //    return default;
        //}

        //public override string ToString()
        //{
        //    return $"{PropertyInfo.Name}:{ParameterAttribute.Label}:{PropertyInfo.PropertyType.Name}";
        //}


        //public void SetPropertyValue<V>(V newValue)
        //{
        //    if (newValue is T tValue)
        //    {
        //        // If the EvaluateType is set, we assume the value is a string and we just set it directly.
        //        if (ParameterAttribute.EvaluateType != null)
        //        {
        //            PropertyInfo.SetValue(_propertyOwner, tValue.ToString());
        //        }
        //        else // If EvaluateType is not set, we assume the value is of type T and we can set it directly.
        //        {
        //            PropertyInfo.SetValue(_propertyOwner, tValue);
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidCastException($"Cannot cast value of type {typeof(V).Name} to {typeof(T).Name}.");
        //    }
        //}
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
