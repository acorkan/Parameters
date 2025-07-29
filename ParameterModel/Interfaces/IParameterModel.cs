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

        Type ParameterType { get; }

        string ParameterName { get; }

        /// <summary>
        /// Set if the parameter supports variable assignment.
        /// </summary>
        bool CanBeVariable { get; }

        bool IsReadOnly { get; }

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
        /// Get the display string for the property and a flag indicating if this is just avariable name.
        /// </summary>
        /// <param name="isVariableAssignment"></param>
        /// <returns></returns>
        string GetDisplayString(out bool isVariableAssignment);

        bool ValidateParameter(List<string> errors);

        /// <summary>
        /// Return possible selection options for a prompt.
        /// Use this for bool, enum, or string[] types.
        /// The variable context it supplied in case the parameter can be assigned to a variable.
        /// </summary>
        /// <returns></returns>
        List<string> GetSelectionItems(IVariablesContext variablesContext);

        bool TestOrAssignVariable(IVariablesContext variablesContext, string varName, bool setVarValue, out string error);

        bool TestOrSetParameter(string newValue, bool setProperty);

        string GetVariableAssignment();
    }
}
