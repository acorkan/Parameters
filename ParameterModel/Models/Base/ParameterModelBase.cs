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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParameterModel.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ParameterModelBase : ModelBase<ParameterModelMessage>, IParameterModel //class ParameterModelBase<T> : ModelBase<ParameterModelMessage>, IParameterModel<T>
    {
        //static ParameterModelBase()
        //{
        //    //??TypeValidator.ValidateType<T>();
        //}

        protected IImplementsParameterAttribute _propertyOwner;
        //protected IStatementEvaluator _statementEvaluator;

        public IVariablesContext VariablesContext { get; }

        public ParameterModelBase(ParameterAttribute parameterPromptAttribute,
            IImplementsParameterAttribute propertyOwner,
            IVariablesContext variablesContext)
        {
            ParameterAttribute = parameterPromptAttribute;
            _propertyOwner = propertyOwner;
            //IsPropertyTypeString = PropertyInfo.PropertyType == typeof(string);
            //CanBeVariable = ParameterAttribute.CanBeVariable;
            VariablesContext = variablesContext;
            if (CanBeVariable && (VariablesContext == null))
            {
                throw new InvalidOperationException($"VariablesContext cannot be null because property {PropertyInfo.Name} has CanBeVariable set.");
            }

            //if (CanBeVariable && !TryGetPropertyValue(out T propertyValue, out string propertyError))
            //{
            //    VariableError = propertyError;
            //}


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


        //protected virtual bool TryParse(string valueString, out T value, out string parseError)
        //{
        //    parseError = "";
        //    value = GetDefault();
        //    if (valueString == null)
        //    {
        //        parseError = "Value string cannot be null.";
        //        return false;
        //    }
        //    if (TryParse(valueString, out value))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        parseError = $"Failed to parse '{valueString}' as a typeof(T) value.";
        //        return false;
        //    }
        //}

        #region IParameterModel
        public ParameterAttribute ParameterAttribute { get; }
        //public bool IsPropertyTypeString { get; }
        public PropertyInfo PropertyInfo { get; }

        public bool CanBeVariable { get => ParameterAttribute.CanBeVariable; }

        public bool IsError { get => Errors.Count != 0; }

        public Type ParameterType { get => ParameterAttribute.PropertyInfo.PropertyType; }

        public string ParameterName => throw new NotImplementedException();

        public List<string> Errors { get; } = new List<string>();

        public bool IsVariableSelected { get => ParameterAttribute.IsVariableSelected; }

        public bool AssignValueString(string valueString, out string errorMessage)
        {
            return ParameterAttribute.TrySetPropertyValue(valueString, out errorMessage);
        }

        public bool GetDisplayString(out string displayString, out bool isVariableAssignment)
        {
            return ParameterAttribute.GetDisplayString(out displayString, out isVariableAssignment);
        }

        public string GetDisplayString()
        {
            ParameterAttribute.GetDisplayString(out string displayString, out bool isVariableAssignment);
            return displayString;
        }

        public bool TestValueString(string valueString, out string parseError)
        {
            return ParameterAttribute.TestPropertyValue(valueString, out parseError);
        }

        //public string GetPropertyValueString()
        //{
        //    if (IsPropertyTypeString)
        //    {
        //        return (string)PropertyInfo.GetValue(_propertyOwner);
        //    }
        //    throw new ArgumentException($"{PropertyInfo.Name} type, it is not a string.");
        //}


        /// <summary>
        /// This returns selections that can be used in a combobox.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetSelectionItems() => Array.Empty<string>(); // Default implementation returns an empty array.

        #endregion IParameterModel
        //        #region IParameterModel<T>
        //        /// <summary>
        //        /// Validate the specified value against the attribute settings.
        //        /// </summary>
        //        /// <param name="val"></param>
        //        /// <param name="attributeError"></param>
        //        /// <returns></returns>
        //        public abstract bool TestAttibuteValidation(T val, out string attributeError);


        //        /// <summary>
        //        /// Return the default value.
        //        /// </summary>
        //        /// <returns></returns>
        //        public virtual T GetDefault()
        //        {
        //            return default(T); // Default implementation returns the default value for type T.
        //        }

        //        //public abstract bool TryGetFormat(T val, out string formattedValue);

        //        public virtual string Format(T val)
        //        {
        //            return val?.ToString() ?? string.Empty; // Default formatting to string representation.
        //        }

        //        public void SetPropertyValue(T newValue)
        //        {
        //            if (IsPropertyTypeString)// && (typeof(T) != typeof(string)))
        //            {
        //                PropertyInfo.SetValue(_propertyOwner, Format(newValue));
        //            }
        //            else
        //            {
        //                PropertyInfo.SetValue(_propertyOwner, newValue);
        //            }
        //        }

        //        public bool TryGetPropertyValue(out T val, out string propertyError)
        //        {
        //            propertyError = "";
        //            val = GetDefault();
        //            AttributeError = null;
        //            if (PropertyInfo.PropertyType == typeof(T))
        //            {
        //                // If the property type is already T, we can just get the value directly.
        //                val = (T)PropertyInfo.GetValue(_propertyOwner);
        //                return true; // Successfully retrieved the value.
        //            }
        //            else if (CanBeVariable) //(IsPropertyTypeString)
        //            {
        //                string propertyString = (string)PropertyInfo.GetValue(_propertyOwner);
        //                // Generate the error message here and clear it if OK.
        //                propertyError = $"Can not resolve parameter {PropertyInfo.Name} to type {typeof(T)} from '{propertyString}'.";

        //                // Maybe it is a variable.
        //                VariableBase variable = VariablesContext.GetVariable(propertyString);
        //                if (variable != null)
        //                {
        //                    propertyString = variable.GetValueAsString(); // Get the value of the variable as a string.
        //                    // Generate the error message if the variable is not of the expected type.
        //                    propertyError = $"Can not resolve parameter {PropertyInfo.Name} to type {typeof(T)} from variable '{variable.Name}' with contents '{propertyString}'.";
        //                }

        //                if (TryParse(propertyString, out val, out propertyError))
        //                {
        //                    propertyError = ""; // Clear the error if parsing was successful.
        //                    return true; // Successfully parsed the value.
        //                }
        //            }
        //            return false; // Failed to get the value or parse it.
        //        }

        //        public abstract bool TryParse(string valString, out T val);

        //        public bool IsValid(List<string> errors)
        //        {
        //            errors.Clear();
        //            if(IsAttributeError)
        //            {
        //                errors.Add(AttributeError);
        //            }
        //            if(IsVariableSelected)
        //            {
        ////                errors.AddRange(_evaluateErrors);
        //            }
        //            return (errors.Count == 0);
        //        }

        //        public void UpdateCurrentAssignmentErrors()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        #endregion IParameterModel<T>

    }


    public class ParameterModelMessage : ModelDependentMessage 
    {
    }
}
