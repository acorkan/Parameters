using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models.Base
{
    public class VariablesContext : IVariablesContext
    {
        public char VariablePrefix { get; } = '%'; // Used to indicate a variable in the string value.

        private static string _varRegexPattern = @"^[A-Za-z_][A-Za-z0-9_]*$";

        public VariablesContext()
        {
        }

        protected System.Text.RegularExpressions.Regex VariableNameRegex { get; } = new System.Text.RegularExpressions.Regex(_varRegexPattern);


        public List<VariableBase> Variables { get; } = new List<VariableBase>();

        public bool RemoveVariable(VariableBase variableBase)
        {
            return Variables.Remove(variableBase);
        }

        public VariableBase AddVariable(string name, VariableType variableType, VariableSource source = VariableSource.Undefined)
        {
            VariableBase existing = GetVariable(name, variableType);
            if (existing != null)
            {
                return existing; // Return existing variable if found
            }
            // Is there a conflict?
            existing = GetVariable(name);
            if (existing != null)
            {
                throw new ArgumentException($"Variable with name '{name}' already exists with a different type {existing.Type}.");
            }
            VariableBase variableBase = new VariableBase(name, variableType);
            Variables.Add(variableBase);
            return variableBase;
        }

        public void ClearVariables()
        {
            Variables.Clear();
        }

        public VariableBase RemoveVariable(string name)
        {
            VariableBase variableBase = GetVariable(name);
            if (variableBase != null)
            {
                Variables.Remove(variableBase);
                return variableBase;
            }
            return null;
        }

        public VariableBase GetVariable(string name)
        {
            return Variables.Find(v => v.Name == name);
        }

        public VariableBase GetVariable(string name, VariableType variableType)
        {
            return Variables.Find(v => (v.Name == name) && (v.Type == variableType));
        }

        public bool IsVariableNameValid(string name)
        {
            if (name.StartsWith(VariablePrefix))
            {
                // Remove the prefix for validation
                name = name.Substring(1);
            }
            // This will throw an exception if the regex is invalid, which is not expected in this context.
            return VariableNameRegex.IsMatch(name); 
        }
    }
}
