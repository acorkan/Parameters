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
    public class StringArrayParameterModel : ParameterModelBase<string[]>
    {
        public static readonly char StrArrayParameterDelimiter = ',';

        public StringArrayParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
        }

        protected override string[] GetDefault()
        {
            return new string[0];
        }

        public override string[] GetSelections()
        {
            return null;
        }

        protected override string FormatType(string[] typeValue)
        {
            return string.Join(StrArrayParameterDelimiter, typeValue);
        }

        protected override bool TryParse(string valueString, out string[] value)
        {
            value = valueString.Split(StrArrayParameterDelimiter, StringSplitOptions.RemoveEmptyEntries);
            return true;
        }

        protected override string ValidateAttibute(string[] val)
        {
            if (ParameterAttribute.AllowEmptyString || (val.Length > 0))
            {
                return null;
            }
            return "Entry cannot be blank";
        }
    }
}
