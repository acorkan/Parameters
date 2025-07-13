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
    /// This interface is for each property that has a ParameterAttribute and will appear in a prompt.
    /// 
    /// If ParameterAttribute.CanBeVariable is set then the property can be assigned from a variable as well.
    /// </summary>
    public interface IParameterModel
    {
        /// <summary>
        /// The attribute for the property.
        /// </summary>
        ParameterAttribute ParameterAttribute { get; }

        IVariablesContext VariablesContext { get; }

        Type ParameterType { get; }

        string ParameterName { get; }

        /// <summary>
        /// Set if the parameter supports variable assignment.
        /// </summary>
        bool CanBeVariable { get; }

        List<string> Errors { get; }

        /// <summary>
        /// Set if Errors is not empty.
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// True if VariableError is not null.
        /// This is updated in the ctor or after SetPropertyValue() or SetPropertyValueString() are called.
        /// </summary>
        bool IsVariableSelected { get; }

        /// <summary>
        /// If IsPropertyTypeString is set then returns the property value, 
        /// otherwise returns the formatted property value in the correct string format.
        /// This is the initial display in a prompt.
        /// </summary>
        /// <returns></returns>
        bool GetDisplayString(out string displayString, out bool isVariableAssignment);

        /// <summary>
        /// Get the display string ignoring errors.
        /// </summary>
        /// <returns></returns>
        string GetDisplayString();

        /// <summary>
        /// Returns true if the string can be assigned to the property type (apply a TryParse() method)
        /// It is allowed to violate any attribute validation rules, they are not tested here.
        /// </summary>
        /// <param name="valueString"></param>
        /// <returns></returns>
        bool TestValueString(string valueString);

        /// <summary>
        /// Assign the string to the property and remove the variable assignment.
        /// The string must be valid for the property type or thows an exception.
        /// This should trigger an update of the Errors list based on annotations.
        /// </summary>
        /// <param name="valueString"></param>
        void AssignValueStringToProperty(string valueString);

        /// <summary>
        /// Assign the variable to the property.
        /// The string must be valid for a variable type.
        /// This should always clear the Errors list.
        /// </summary>
        /// <param name="valueString"></param>
        void AssignVaribleToProperty(string valueString);

        /// <summary>
        /// Return possible selection options for a prompt.
        /// Use this for bool, enum, or string[] types.
        /// </summary>
        /// <returns></returns>
        string[] GetSelectionItems();
    }
}
