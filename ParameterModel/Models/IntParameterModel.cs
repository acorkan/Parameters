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
    public class IntParameterModel : ParameterModelBase
    {
        public IntParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, variablesContext)
        {
            //if (IsVariableAllowed &&  !TryGetPropertyValue(out bool propertyValue, out string propertyError))
            //{
            //    VariableError = propertyError;
            //}
        }

        //public override string Format(int val)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int GetDefault()
        //{
        //    throw new NotImplementedException();
        //}

        //public override string[] GetSelectionItems() => [];

        //public override bool TestAttibuteValidation(int val, out string attributeError)
        //{
        //    attributeError = "";
        //    if (ParameterAttribute.Min != ParameterAttribute.Max)
        //    {
        //        if (val < (int)ParameterAttribute.Min)
        //        {
        //            attributeError = $"Value must be greater than or equal to {(int)ParameterAttribute.Min}";
        //        }
        //        if (i > (int)ParameterAttribute.Max)
        //        {
        //            attributeError = $"Value must be less than or equal to {(int)ParameterAttribute.Max}";
        //        }
        //    }
        //    return string.IsNullOrEmpty(attributeError);
        //}

        //public override bool TryParse(string valString, out int val)
        //{
        //    return int.TryParse(valString, out val);
        //}
    }
}
