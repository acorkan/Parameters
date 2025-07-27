using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterModel.Variables;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models
{
    public class VariableParameterModel : ParameterModelBase
    {
        public VariableAccessType AccessType { get; } = VariableAccessType.ReadWrite;

        public override VariableType[] AllowedVariableTypes { get; }

        public VariableParameterModel(ParameterAttribute parameterPromptAttribute) : 
            base(parameterPromptAttribute)
        {
            VariableAssignmentAttribute dfAttrib = ParameterAttribute.PropertyInfo.GetCustomAttribute<VariableAssignmentAttribute>();
            if (dfAttrib != null)
            {
                AccessType = dfAttrib.VariableAccess;
                AllowedVariableTypes = dfAttrib.VariableTypes ?? Array.Empty<VariableType>();
            }
        }

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if(VariablesContext.IsVariableNameValid(newValue))
            {
                if (setProperty)
                {
                    VariableProperty vp = (VariableProperty)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
                    if(AccessType.Equals(VariableAccessType.ReadWrite))
                    {
                        vp.Assignment = newValue;
                        return true;
                    }
                }
            }
            return false;
        }

        protected override string GetDisplayString()
        {
            return ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes).ToString();
        }
    }
}
