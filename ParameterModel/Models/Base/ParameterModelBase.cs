using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
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
        //static ParameterModelBase()
        //{
        //    //??TypeValidator.ValidateType<T>();
        //}

        protected IImplementsParameterAttribute _propertyOwner;
        //protected IStatementEvaluator _statementEvaluator;

        public IVariablesContext VariablesContext { get; }

        public ParameterModelBase(ParameterAttribute parameterPromptAttribute, 
            PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner,
            IVariablesContext variablesContext)
        {
            ParameterAttribute = parameterPromptAttribute;
            PropertyInfo = propertyInfo;
            _propertyOwner = propertyOwner;
            IsPropertyTypeString = PropertyInfo.PropertyType == typeof(string);
            IsVariableAllowed = ParameterAttribute.VariableType != null;
            VariablesContext = variablesContext;
            if (IsVariableAllowed && (VariablesContext == null))
            {
                throw new InvalidOperationException($"VariablesContext cannot be null for property {PropertyInfo.Name} VariableType set to {ParameterAttribute.VariableType}.");
            }

            if (IsVariableAllowed && !TryGetPropertyValue(out T propertyValue, out string propertyError))
            {
                VariableError = propertyError;
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

        public bool IsVariableAllowed { get; }

        public string AttributeError { get; protected set; }

        public bool IsAttributeError { get => !string.IsNullOrEmpty(AttributeError); }

        public string ParameterError { get; set; }

        public bool IsParameterError { get => !string.IsNullOrEmpty(ParameterError);  }

        public string VariableError { get; protected set; }

        public bool IsVariableError { get => !string.IsNullOrEmpty(VariableError); }

        public bool IsAnyError { get => IsAttributeError || IsParameterError || IsVariableError; }

        public Type ParameterType { get => IsVariableAllowed ? ParameterAttribute.VariableType : PropertyInfo.PropertyType; }

        public virtual string ToDisplayString()
        {
            if (IsPropertyTypeString) // && PropertyInfo.GetValue(_propertyOwner) is string val)
            {
                return (string)PropertyInfo.GetValue(_propertyOwner) ?? string.Empty;
            }
            else
            {
                T val = (T)PropertyInfo.GetValue(_propertyOwner);
                return Format(val);
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
        public virtual string[] GetSelectionItems() => Array.Empty<string>(); // Default implementation returns an empty array.

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
        public virtual T GetDefault()
        {
            return default(T); // Default implementation returns the default value for type T.
        }

        //public abstract bool TryGetFormat(T val, out string formattedValue);

        public virtual string Format(T val)
        {
            return val?.ToString() ?? string.Empty; // Default formatting to string representation.
        }

        public void SetPropertyValue(T newValue)
        {
            if (IsPropertyTypeString)// && (typeof(T) != typeof(string)))
            {
                PropertyInfo.SetValue(_propertyOwner, Format(newValue));
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
            if (PropertyInfo.PropertyType == typeof(T))
            {
                // If the property type is already T, we can just get the value directly.
                val = (T)PropertyInfo.GetValue(_propertyOwner);
                return true; // Successfully retrieved the value.
            }
            else if (IsVariableAllowed) //(IsPropertyTypeString)
            {
                string propertyString = (string)PropertyInfo.GetValue(_propertyOwner);
                // Generate the error message here and clear it if OK.
                propertyError = $"Can not resolve parameter {PropertyInfo.Name} to type {typeof(T)} from '{propertyString}'.";

                // Maybe it is a variable.
                VariableBase variable = VariablesContext.GetVariable(propertyString);
                if (variable != null)
                {
                    propertyString = variable.GetValueAsString(); // Get the value of the variable as a string.
                    // Generate the error message if the variable is not of the expected type.
                    propertyError = $"Can not resolve parameter {PropertyInfo.Name} to type {typeof(T)} from variable '{variable.Name}' with contents '{propertyString}'.";
                }

                if (TryParse(propertyString, out val, out propertyError))
                {
                    propertyError = ""; // Clear the error if parsing was successful.
                    return true; // Successfully parsed the value.
                }
            }
            return false; // Failed to get the value or parse it.
        }

        public abstract bool TryParse(string valString, out T val);

        public bool IsValid(List<string> errors)
        {
            errors.Clear();
            if(IsAttributeError)
            {
                errors.Add(AttributeError);
            }
            if(IsVariableError)
            {
//                errors.AddRange(_evaluateErrors);
            }
            return (errors.Count == 0);
        }

        public void UpdateCurrentAssignmentErrors()
        {
            throw new NotImplementedException();
        }
        #endregion IParameterModel<T>

  
    }

    public class ParameterModelMessage : ModelDependentMessage { }
     
}
