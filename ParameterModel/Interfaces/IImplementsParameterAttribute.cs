using ParameterModel.Attributes;
using System.Reflection;

namespace ParameterModel.Interfaces
{
    /// <summary>
    /// Marker interface.
    /// Add this to any class with properties that will be attributed with ParameterPromptAttribute.
    /// </summary>
    public interface IImplementsParameterAttribute
    {
        /// <summary>
        /// Used by the ImplementParameterAttributeExtension class to track variable assignments for properties that are 
        /// attributed with ParameterPromptAttribute and for which IsVariable = true. 
        /// POroiperty name is Key, and variable name is Value.
        /// </summary>
        Dictionary<string, string> VariableAssignments { get; }
        /// <summary>
        /// Initialized and used by the ImplementParameterAttributeExtension class to track errors for properties that are
        /// attributed with ParameterPromptAttribute.
        /// </summary>
        Dictionary<string, ParameterAttribute> AttributeMap { get; }
    }
}
