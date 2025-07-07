using log4net;
using ParameterModel.Interfaces;
using ParameterModel.Variables;

namespace ParameterModel.Models.Base
{
    public class EvaluationContextBase : IEvaluationContext
    {
        public ILog Log { get; private set; }
        public bool IsDebug { get; private set; }
        public bool IsSimulation { get; private set; }

        List<VariableBase> Variables { get; } = new List<VariableBase>();

        List<VariableBase> IVariablesContext.Variables => Variables;

        public EvaluationContextBase(ILog log, bool isDebug, bool isSimulation)
        {
            //Log = log ?? throw new ArgumentNullException(nameof(log));
            IsDebug = isDebug;
            IsSimulation = isSimulation;
        }

        public VariableBase GetVariable(string name)
        {
            return Variables.Find(v => v.Name == name);
        }

        public VariableBase GetVariable(string name, VariableType variableType)
        {
            return Variables.Find(v => (v.Name == name) && (v.Type == variableType));
        }

        public VariableBase DeleteVariable(string name)
        {
            VariableBase variableBase = GetVariable(name);
            if(variableBase != null)
            {
                Variables.Remove(variableBase);
                return variableBase;
            }
            return null;
        }

        public bool RemoveVariable(VariableBase variableBase)
        {
            return Variables.Remove(variableBase);
        }

        public VariableBase AddVariable(string name, VariableType variableType, VariableSource source = VariableSource.Undefined)
        {
            VariableBase existing = GetVariable(name, variableType);
            if(existing != null)
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
    }
}
