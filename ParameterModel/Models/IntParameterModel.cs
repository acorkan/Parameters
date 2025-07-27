using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models
{
    public class IntParameterModel : ParameterModelBase
    {
        public int Min { get; } = -1;
        public int Max { get; } = -1;
        public bool IsMinExclusive { get; } = false;
        public bool IsMaxExclusive { get; } = false;
        public string DataFormatString { get; } = null;


        public IntParameterModel(ParameterAttribute parameterPromptAttribute) : 
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

        public override VariableType[] AllowedVariableTypes => [VariableType.Integer];

        protected override string GetDisplayString()
        {
            int i = (int)ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return i.ToString();
        }

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if (int.TryParse(newValue, out int i))
            {
                if (setProperty)
                {
                    ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, i);
                }
                return true;
            }
            return false;
        }
    }
}
