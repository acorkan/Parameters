using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    public enum VariableType
    {
        Undefined,
        Integer,
        Float,
        String,
        Boolean,
        JSON
    }

    public enum VariableSource
    {
        Undefined,
        UserDefined,
        SystemDefined,
        ExternalSource
    }

    public interface IVariablesContext
    {
        /// <summary>
        /// All variables in the context.
        /// </summary>
        List<VariableBase> Variables { get; }

        /// <summary>
        /// Add a new variable with the specified name, type, and source.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variableType"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        VariableBase AddVariable(string name, VariableType variableType, VariableSource source = VariableSource.Undefined);

        /// <summary>
        /// Get by case-sensitive name or return null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        VariableBase GetVariable(string name);

        /// <summary>
        /// Get by case-sensitive name and type, or return null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        VariableBase GetVariable(string name, VariableType variableType);

        /// <summary>
        /// Delete by case-sensitive name. If found return value or return null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        VariableBase DeleteVariable(string name);

        /// <summary>
        /// Delete by case-sensitive name. If found return value or return null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveVariable(VariableBase variableBase);

        void ClearVariables();
    }
}
