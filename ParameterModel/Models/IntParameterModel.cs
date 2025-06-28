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
    public class IntParameterModel : ParameterModelBase<int>
    {
        public IntParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) 
            : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
        }
        protected override int GetDefault()
        {
            return 0;
        }
        public override string[] GetSelections() 
        {
            return null; // Int parameters do not have selections like enums or strings.
        }
        protected override string FormatType(int typeValue)
        {
            return typeValue.ToString();
        }
        protected override bool TryParse(string valueString, out int value)
        {
            return int.TryParse(valueString, out value);
        }
        protected override string ValidateAttibute(int i)
        {
            if (ParameterAttribute.Min != ParameterAttribute.Max)
            {
                if (i < ParameterAttribute.Min)
                {
                    return $"Value must be greater than or equal to {ParameterAttribute.Min}";
                }
                if (i > ParameterAttribute.Max)
                {
                    return $"Value must be less than or equal to {ParameterAttribute.Max}";
                }
            }
            return null;
        }
    }
}
