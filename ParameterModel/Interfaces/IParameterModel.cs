using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    /// <summary>
    /// Parameter can be string, string[], bool, float, int, or enum.
    /// The parameter can also be assigned from a variable. If assigned from a variable 
    /// then the underlying property type must be a string, otherwise the proerty type matched the parameter type.
    /// 
    /// Therefore if ParameterAttribute.VariableType is set then the underlying property type must be a string and 
    /// in the UI the string must be limited to either a literal that can be parsed to ParameterAttribute.VariableType or a
    /// variable type that is compatible with ParameterAttribute.VariableType.
    /// </summary>
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

        IVariablesContext VariablesContext { get; }

        /// <summary>
        /// If IsVariableAllowed then this is the ParameterAttribute.VariableType,
        /// otherwise it is the PropertyInfo.PropertyType.
        /// </summary>
        Type ParameterType { get; }

        /// <summary>
        /// Set if underlying property is a string.
        /// </summary>
        bool IsPropertyTypeString { get; }

        /// <summary>
        /// Set if the parameter supports variable assignment.
        /// </summary>
        bool IsVariableAllowed { get; }

        /// <summary>
        /// Set if the property value violates the attribute rules.
        /// This is updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        string AttributeError { get; }

        /// <summary>
        /// True if AttributeError is not null.
        /// This is updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        bool IsAttributeError { get; }

        /// <summary>
        /// Set if the property value can be evaluated as a variable, and there was a problem evaluating it.
        /// This is updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        string VariableError { get; }

        /// <summary>
        /// True if VariableError is not null.
        /// This is updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        bool IsVariableError { get; }


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
        /// Call to test if the variable assignement violates attribute parameters or variable assignment type.
        /// Call in ctor and after SetPropertyValue() or SetPropertyValueString() are called.
        /// Also call when the VariablesContext is updated to check if the variable assignment is valid.
        /// </summary>
        /// <param name="variablesContext"></param>
        /// <returns></returns>
        void UpdateCurrentAssignmentErrors();

        /// <summary>
        /// Set if either IsParameterError, IsEvaluateError, or IsAttributeError is set.
        /// </summary>
        bool IsAnyError { get; }

        /// <summary>
        /// If IsPropertyTypeString is set then returns the property value, 
        /// otherwise returns the formatted property value in the correct string format.
        /// This is the initial display in a prompt.
        /// </summary>
        /// <returns></returns>
        string ToDisplayString();

        /// <summary>
        /// Returns true if the string can be assigned to the property, and does not violate any attribute validation rules.
        /// If the string is a variable reference then the variable must already exist and be compatible with the property type.
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="parseError"></param>
        /// <returns></returns>
        bool TestValueString(string valueString, out string parseError);

        /// <summary>
        /// Assign the string to the property.
        /// If the type does not support variable assignement then it must parse to the property type, otherwise it throws an argument exception.
        /// If the string is not a single token or null it will throw an argument exception.
        /// If the type does support variable assignement then the value is written to the string property.
        /// Call UpdateCurrentAssignmentErrors() after this to update the error state of the parameter.
        /// </summary>
        /// <param name="newValueString"></param>
        void SetPropertyValueString(string newValueString);

        ///// <summary>
        ///// Return the property value if the property type is string.
        ///// Throws exception if not a string type.
        ///// </summary>
        ///// <returns></returns>
        //string GetPropertyValueString();

        /// <summary>
        /// Return possible selection options for a prompt.
        /// Use this for bool, enum, or string[] types.
        /// </summary>
        /// <returns></returns>
        string[] GetSelectionItems();

        /// <summary>
        /// If a variable is assigned to the property, then this will resolve the variable and assign the value to the property.
        /// 
        /// </summary>
        /// <param name="variablesContext"></param>
        /// <param name="propertyOwner"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ResolveVariable(IVariablesContext variablesContext, IImplementsParameterAttribute propertyOwner)
        {
            if (IsVariable)
            {
                // If the variable type is set, we will try to resolve it.
                if (variablesContext.TryGetVariableValue(VariableType, out object? value, out string error))
                {
                    PropertyInfo.SetValue(propertyOwner, value);
                }
                else
                {
                    throw new InvalidOperationException($"Failed to resolve variable for {PropertyInfo.Name}: {error}");
                }
            }
        }

    }

    public interface IParameterModel<T> : IParameterModel
    {
 
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
        string Format(T val);

        /// <summary>
        /// Assign a value to the property.
        /// If the type does support variable assignement then the Format(value) is written to the string property.
        /// Call UpdateCurrentAssignmentErrors() after this to update the error state of the parameter.
        /// </summary>
        /// <param name="newValue"></param>
        void SetPropertyValue(T newValue);

        /// <summary>
        /// Attempts to retrieve a property value or an associated error message, if any, from the current context.
        /// If the type does support variable assignement then the string property must pass parsing or it must be a variable of the correct type.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="propertyError"></param>
        /// <returns></returns>
        bool TryGetPropertyValue(out T val, out string propertyError);

        /// <summary>
        /// Parse the string and see if it can be assigned to an instance of the property type.
        /// Does not change the property value.
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        bool TryParse(string valString, out T val);
    }
}
