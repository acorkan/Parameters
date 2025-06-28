using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
    public class FloatParameterModel : ParameterModelBase<float>
    {
        public FloatParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
        }

        protected override float GetDefault()
        {
            return 0.0F;
        }

        public override string[] GetSelections() 
        {
            return null; // Float parameters do not have selections like enums or strings.
        }

        protected override string FormatType(float typeValue)
        {
            return typeValue.ToString("F2");
        }

        protected override bool TryParse(string valueString, out float value)
        {
            return float.TryParse(valueString, out value);
        }

        protected override string ValidateAttibute(float f)
        {
            if (ParameterAttribute.Min != ParameterAttribute.Max)
            {
                if (f < ParameterAttribute.Min)
                {
                    return $"Value must be greater than or equal to {ParameterAttribute.Min}";
                }
                if (f > ParameterAttribute.Max)
                {
                    return $"Value must be less than or equal to {ParameterAttribute.Max}";
                }
            }
            return null;
        }
    }
}
