using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParameterModel.Models
{
    public class EnumParameterModel : ParameterModelBase
    {
        public EnumParameterModel(ParameterAttribute parameterPromptAttribute) : 
            base(parameterPromptAttribute)
        {
            _defaultSelections = parameterPromptAttribute.GetEnumItemsDisplay().Values.ToArray();
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.String];

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            Enum value = null;
            string valueString = null;
            Dictionary<Enum, string> enumItemsDisplayDict = ParameterAttribute.GetEnumItemsDisplay();
            for (int i = 0; i < enumItemsDisplayDict.Count; i++)
            {
                if (enumItemsDisplayDict.ElementAt(i).Key.ToString().Equals(newValue, StringComparison.OrdinalIgnoreCase) ||
                    enumItemsDisplayDict.ElementAt(i).Value.Equals(newValue, StringComparison.OrdinalIgnoreCase))
                {
                    value = enumItemsDisplayDict.ElementAt(i).Key;
                    break;
                }
            }
            if (value != null)
            {
                if (Enum.TryParse(ParameterAttribute.PropertyInfo.PropertyType, valueString, true, out object e))
                {
                    if (setProperty)
                    {
                        ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, e);
                    }
                    return true;
                }
                throw new InvalidOperationException($"Parameter '{ParameterName}' could not be parsed as enum '{ParameterAttribute.PropertyInfo.PropertyType}'.");
            }
            throw new InvalidOperationException($"Can not resolve parameter {ParameterName} from enum type {ParameterAttribute.PropertyInfo.PropertyType}");
        }

        protected override string GetDisplayString()
        {
            string displayString;
            if (ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes) is Enum typeValue)
            {
                Dictionary<Enum, string> enumItemsDisplayDict = ParameterAttribute.GetEnumItemsDisplay();
                if (enumItemsDisplayDict.ContainsKey(typeValue))
                {
                    displayString = enumItemsDisplayDict[typeValue];
                }
                else
                {
                    displayString = typeValue.ToString();
                }
                return displayString;
            }
            throw new InvalidOperationException($"Can not resolve property {ParameterName} from enum type {ParameterAttribute.PropertyInfo.PropertyType}");
        }
    }
}
