using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models
{
    public class FloatParameterModel : ParameterModelBase
    {
        protected readonly string _displayFormat;
        public FloatParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) :
            base(parameterPromptAttribute, variablesContext)
        {
            // Access the display format if provided.
            DisplayFormatAttribute formatAttribute = ParameterAttribute.PropertyInfo.GetCustomAttribute<DisplayFormatAttribute>();
            if (formatAttribute != null)
            {
                _displayFormat = formatAttribute.DataFormatString;
            }
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.Integer, VariableType.Float];

        public override bool TestOrSetSetPropertyValue(string newValue, bool setProperty)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplayString()
        {
            float f = (int)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return string.IsNullOrEmpty(_displayFormat) ? f.ToString() : f.ToString(_displayFormat);
        }
    }
}
