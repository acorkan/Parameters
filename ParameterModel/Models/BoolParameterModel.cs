using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParameterModel.Models
{
    public class BoolParameterModel : ParameterModelBase//<bool>
    {

        //protected override bool TryParse(string valueString, out bool value)
        //{
        //    return bool.TryParse(valueString.ToLower(), out value);
        //}

        //protected override string TestAttibuteValidation(bool val)
        //{
        //    return null;
        //}

        private readonly string[] _selections = { "True", "False" };

        public BoolParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
                base(parameterPromptAttribute, variablesContext)
        {
            //if (IsVariableAllowed && !TryGetPropertyValue(out bool propertyValue, out string propertyError))
            //{
            //    VariableError = propertyError;
            //}
        }

        public override string[] GetSelectionItems()
        {
            return _selections;
        }


        //public override string Format(bool val)
        //{
        //    return val.ToString();
        //}

        //public override bool GetDefault()
        //{
        //    return false;
        //}

        //protected override bool TryParse(string valueString, out bool value, out string parseError)
        //{
        //    parseError = "";
        //    if (valueString == null)
        //    {
        //        parseError = "Value string cannot be null.";
        //        value = false;
        //        return false;
        //    }
        //    if (bool.TryParse(valueString, out value))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        parseError = $"Failed to parse '{valueString}' as a boolean value.";
        //        value = false;
        //        return false;
        //    }
        //}

        //public override string Format()
        //{
        //    if (IsPropertyTypeString)
        //    {
        //        string val = (string)PropertyInfo.GetValue(_propertyOwner);
        //        if(bool.TryParse(val, out bool boolVal))
        //        {
        //            return boolVal ? "True" : "False";
        //        }
        //        else
        //        {
        //            return "Invalid boolean value";
        //        }
        //    }
        //    return null;
        //}


        //public override string Format()
        //{
        //    if (IsPropertyTypeString) // && PropertyInfo.GetValue(_propertyOwner) is string val)
        //    {
        //        string propertyString = (string)PropertyInfo.GetValue(_propertyOwner) ?? string.Empty;
        //        if (_statementEvaluator != null)
        //        {
        //            List<string> errors = new List<string>();
        //            if (_statementEvaluator.TryEvaluate(propertyString, out string result, errors))
        //            {
        //                propertyString = result;
        //            }
        //        }
        //        if (bool.TryParse(propertyString, out bool boolVal))
        //        {
        //            return boolVal ? "True" : "False";
        //        }
        //        else
        //        {
        //            return null;// "Invalid boolean value";
        //        }
        //    }
        //    else
        //    {
        //        bool boolVal = (bool)PropertyInfo.GetValue(_propertyOwner);
        //        return boolVal ? "True" : "False";
        //    }



        //public override bool TryParse(string valString, out bool val)
        //{
        //    return bool.TryParse(valString, out val);
        //}

        //public override bool TestAttibuteValidation(bool val, out string attributeError)
        //{
        //    attributeError = "";
        //    return true; // No specific validation for boolean values, but can be overridden if needed.
        //}

        //public override bool TestAttibuteValidation(bool val, out string attributeError)
        //{
        //    if (TryGetValue(val, out string propertyError))
        //    {
        //        if (val == null)
        //        {
        //            attributeError = "Value cannot be null.";
        //            return false;
        //        }
        //        if (val is bool booleanValue)
        //        {
        //            attributeError = TestAttibuteValidation(booleanValue);
        //            return string.IsNullOrEmpty(attributeError);
        //        }
        //        else
        //        {
        //            attributeError = $"Value '{val}' is not a boolean.";
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        attributeError = propertyError;
        //        return false;

        //    }
        //}

        //public override void SetValue<T>(T newValue)
        //{
        //    if (IsPropertyTypeString && (typeof(T) != typeof(string)))
        //    {
        //        throw new ArgumentException($"Property for {PropertyInfo.Name} is not a string type");
        //    }
        //    else
        //    {
        //        PropertyInfo.SetValue(_propertyOwner, newValue);
        //    }
        //}

        //public override void SetValueString(string newValueString)
        //{
        //    if (IsPropertyTypeString)
        //    {
        //        PropertyInfo.SetValue(_propertyOwner, newValueString);
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"Property for {PropertyInfo.Name} is not a string type");
        //    }
        //}
    }
}
