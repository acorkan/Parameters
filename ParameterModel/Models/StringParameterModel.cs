using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ParameterModel.Models
{
    public class StringParameterModel : ParameterModelBase<string>
    {
        public StringParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) 
            : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
        }
        protected override string GetDefault()
        {
            return string.Empty;
        }
        public override string[] GetSelections() 
        {
            return null; // String parameters do not have selections like enums or strings.
        }
        protected override string FormatType(string typeValue)
        {
            return typeValue;
        }
        protected override bool TryParse(string valueString, out string value)
        {
            value = valueString;
            return true;
        }
        protected override string ValidateAttibute(string s)
        {
            if (!ParameterAttribute.AllowEmptyString && string.IsNullOrEmpty(s))
            {
                return "Entry cannot be blank";
            }
            return null;
        }
    }
}
