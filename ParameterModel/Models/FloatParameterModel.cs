using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models
{
    public class FloatParameterModel : ParameterModelBase
    {
        public float Min { get; } = -1;
        public float Max { get; } = -1;
        public bool IsMinExclusive { get; } = false;
        public bool IsMaxExclusive { get; } = false;
        public string DataFormatString { get; } = null;
        public FloatParameterModel(ParameterAttribute parameterPromptAttribute) :
            base(parameterPromptAttribute)
        {
            RangeAttribute attrib = ParameterAttribute.PropertyInfo.GetCustomAttribute<RangeAttribute>();
            if (attrib != null)
            {
                Min = (int)(attrib?.Minimum ?? -1);
                Max = (int)(attrib?.Maximum ?? -1);
                IsMinExclusive = attrib?.MinimumIsExclusive ?? false;
                IsMaxExclusive = attrib?.MaximumIsExclusive ?? false;
            }
            DisplayFormatAttribute dfAttrib = ParameterAttribute.PropertyInfo.GetCustomAttribute<DisplayFormatAttribute>();
            if (dfAttrib != null)
            {
                DataFormatString = dfAttrib.DataFormatString ?? "";
            }
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.Integer, VariableType.Float];

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            throw new NotImplementedException();
        }

        protected override string GetDisplayString()
        {
            float f = (int)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return string.IsNullOrEmpty(DataFormatString) ? f.ToString() : f.ToString(DataFormatString);
        }
    }
}
