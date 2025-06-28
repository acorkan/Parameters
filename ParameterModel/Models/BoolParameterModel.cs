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
    public class BoolParameterModel : ParameterModelBase<bool>
    {
        public BoolParameterModel(ParameterAttribute parameterPromptAttribute, PropertyInfo propertyInfo, IImplementsParameterAttribute propertyOwner) : base(parameterPromptAttribute, propertyInfo, propertyOwner)
        {
        }

        protected override bool GetDefault()
        {
            return false;
        }

        protected override string FormatType(bool typeValue)
        {
            return typeValue ? "True" : "False";
        }

        protected override bool TryParse(string valueString, out bool value)
        {
            return bool.TryParse(valueString.ToLower(), out value);
        }

        protected override string ValidateAttibute(bool val)
        {
            return null;
        }

        private readonly string[] _selections = new string[] { "True", "False" };
        public override string[] GetSelections()
        {
            return _selections;
        }

    }
}
