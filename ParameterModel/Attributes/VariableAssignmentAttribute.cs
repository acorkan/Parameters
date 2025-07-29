using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Attributes
{
    public enum VariableAccessType
    {
        /// <summary>
        /// Means any variable can be selected because the value of the variable will be used.
        /// </summary>
        ReadWrite,
        /// <summary>
        /// Means the selected variable will be modified and therefore it can not be a read-only.
        /// </summary>
        WriteOnly
    }

    /// <summary>
    /// This attribute regulates the assignment of a variable type to a property of type VariableProperty.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public  class VariableAssignmentAttribute : ParameterAttribute
    {
        public VariableAccessType VariableAccess { get; private set; } = VariableAccessType.ReadWrite;
        public VariableType[] VariableTypes { get; private set; }
        public VariableAssignmentAttribute(VariableType variableType, VariableAccessType variableAccess = VariableAccessType.ReadWrite) : 
            this([variableType], variableAccess) { }
        public VariableAssignmentAttribute(VariableType[] variableTypes, VariableAccessType variableAccess = VariableAccessType.ReadWrite) :
            base(false)
        {
            VariableTypes = variableTypes;
            VariableAccess = variableAccess;
        }
    }
}
