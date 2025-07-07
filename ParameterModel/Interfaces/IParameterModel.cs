using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    public interface IParameterModel
    {
        /// <summary>
        /// The attribute for the property.
        /// </summary>
        ParameterAttribute ParameterAttribute { get; }

        /// <summary>
        /// Property access info for the property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Set if underlying property is a string.
        /// </summary>
        bool IsPropertyTypeString { get; }

        /// <summary>
        /// Set if the parameter supports evaluation.
        /// </summary>
        bool CanEvaluate { get; }

        /// <summary>
        /// Set if the property value violates the attribute rules.
        /// Should be updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        string AttributeError { get; }

        /// <summary>
        /// True if AttributeError is not null.
        /// </summary>
        bool IsAttributeError { get; }

        /// <summary>
        /// Set externaly.
        /// Can be set to indicate that there is an error for the parameter.
        /// Intended to be set externally if other related parameters have incompatible settings.
        /// </summary>
        string ParameterError { get; set; }

        /// <summary>
        /// True if ParameterError is not null.
        /// </summary>
        bool IsParameterError { get; }

        /// <summary>
        /// Set if CanEvaluate is set but the evaluation caused an error.
        /// Set at ctor and when SetPropertyValue() or SetPropertyValueString() is called.
        /// </summary>
        bool IsEvaluateError { get; }

        /// <summary>
        /// Set if either IsParameterError, IsEvaluateError, or IsAttributeError is set.
        /// </summary>
        bool IsAnyError { get; }

        bool IsValid(List<string> errors);

        /// <summary>
        /// If IsPropertyTypeString is set then returns the property value, 
        /// otherwise returns the formatted property value in the correct string format.
        /// This is the initial display in a prompt.
        /// </summary>
        /// <returns></returns>
        string ToDisplayString();

        /// <summary>
        /// Returns true if the string can be assigned to the property
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="parseError"></param>
        /// <returns></returns>
        bool TestValueString(string valueString, out string parseError);

        /// <summary>
        /// Assign the string to the property if the property type is string.
        /// Throws exception if not a string type.
        /// </summary>
        /// <param name="newValueString"></param>
        void SetPropertyValueString(string newValueString);

        /// <summary>
        /// Return the property value if the property type is string.
        /// Throws exception if not a string type.
        /// </summary>
        /// <returns></returns>
        string GetPropertyValueString();

        /// <summary>
        /// Return possible selection optionss for a prompt.
        /// </summary>
        /// <returns></returns>
        string[] GetSelectionItems();
    }

    public interface IParameterModel<T> : IParameterModel
    {
        /// <summary>
        /// Test if the specified value passes the attribute parameters.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="attributeError"></param>
        /// <returns></returns>
        bool TestAttibuteValidation(T val, out string attributeError);

        /// <summary>
        /// Return the default value of the parammeter.
        /// </summary>
        /// <returns></returns>
        T GetDefault();

        /// <summary>
        /// Return the display string of the specified value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        string ToDisplayString(T val);

        /// <summary>
        /// Assign a value to the property.
        /// </summary>
        /// <param name="newValue"></param>
        void SetPropertyValue(T newValue);

        /// <summary>
        /// Attempts to retrieve a property value or an associated error message, if any, from the current context.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="propertyError"></param>
        /// <returns></returns>
        bool TryGetPropertyValue(out T val, out string propertyError);

        /// <summary>
        /// Parse the string and see if it can be assigned to an instance of the property type.
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool TryParse(string valString, out T val);
    }
}
