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
            _defaultSelections = parameterPromptAttribute.EnumItemsDisplayDict.Values.ToArray();
        }

        public override VariableType[] AllowedVariableTypes => [VariableType.String];

        public override bool TestOrSetParameter(string newValue, bool setProperty)
        {
            //Enum value = null;
            string selection = null;
            // First test for int value.
            if (int.TryParse(newValue, out int index) && ParameterAttribute.EnumIntDisplayDict.ContainsKey(index))
            {
                selection = ParameterAttribute.EnumIntDisplayDict[index];
            }
            else
            {
                Dictionary<Enum, string> enumItemsDisplayDict = ParameterAttribute.EnumItemsDisplayDict;
                for (int i = 0; i < enumItemsDisplayDict.Count; i++)
                {
                    if (enumItemsDisplayDict.ElementAt(i).Key.ToString().Equals(newValue, StringComparison.OrdinalIgnoreCase) ||
                        enumItemsDisplayDict.ElementAt(i).Value.Equals(newValue, StringComparison.OrdinalIgnoreCase))
                    {
                        selection = enumItemsDisplayDict.ElementAt(i).Value;
                        index = ParameterAttribute.EnumIntDisplayDict.ElementAt(i).Key;
                        break;
                    }
                }
            }
            if (selection != null)
            {
                //if (Enum.TryParse(ParameterAttribute.PropertyInfo.PropertyType, newValue, true, out object e))
                {
                    if (setProperty)
                    {
                        ParameterAttribute.PropertyInfo.SetValue(ParameterAttribute.ImplementsParameterAttributes, index);
                    }
                    return true;
                }
                throw new InvalidOperationException($"Parameter '{ParameterName}' could not be parsed as enum '{ParameterAttribute.PropertyInfo.PropertyType}'.");
            }
            throw new InvalidOperationException($"Can not resolve parameter {ParameterName} from enum type {ParameterAttribute.PropertyInfo.PropertyType}");
        }

        protected override string GetDisplayString()
        {
            if (ParameterAttribute.PropertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes) is Enum typeValue)
            {
                int index = Convert.ToInt32(typeValue);
                //Dictionary<Enum, string> enumItemsDisplayDict = ParameterAttribute.EnumItemsDisplayDict;
                if (ParameterAttribute.EnumIntDisplayDict.ContainsKey(index))
                {
                    return ParameterAttribute.EnumIntDisplayDict[index];
                }
            }
            throw new InvalidOperationException($"Can not resolve property {ParameterName} from enum type {ParameterAttribute.PropertyInfo.PropertyType}");
        }
    }
}
