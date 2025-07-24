using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
    public class StringArrayParameterModel : ParameterModelBase
    {
        public static readonly char StrArrayParameterDelimiter = ' ';
        private readonly char[] _delimiter = [StrArrayParameterDelimiter];

        public StringArrayParameterModel(ParameterAttribute parameterPromptAttribute) : 
            base(parameterPromptAttribute)
        {
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.String];

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            if (newValue == null)
            {
                return false;
            }
            if (setProperty)
            {
                string[] stringArray = newValue.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
                ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, stringArray);
            }
            return true;
        }

        protected override string GetDisplayString()
        {
            string[] stringArray = (string[])ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            return string.Join(StrArrayParameterDelimiter, stringArray);
        }
    }
}
