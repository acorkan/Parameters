using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models
{
    public class StringParameterModel : ParameterModelBase
    {
        public int MinLength { get; } = -1;
        public int MaxLength { get; } = -1;

        public StringParameterModel(ParameterAttribute parameterPromptAttribute)
            : base(parameterPromptAttribute)
        {
            StringLengthAttribute editableAttribute = ParameterAttribute.PropertyInfo.GetCustomAttribute<StringLengthAttribute>();
            MinLength = editableAttribute?.MinimumLength ?? -1;
            MaxLength = editableAttribute?.MaximumLength ?? -1;
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.String, VariableType.JSON];

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if(newValue == null)
            {
                return false;
            }
            if (setProperty)
            {
                ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, newValue);
            }
            return true;
        }

        protected override string GetDisplayString()
        {
            return ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes).ToString();
        }
    }
}
