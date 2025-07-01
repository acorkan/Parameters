
namespace ParameterModel.Models
{
 /*   internal static class TypeValidator
    {
        public static Func<T, ParameterAttribute, string> GetValidateType<T>()
        {
            var type = typeof(T);
            if (type == typeof(string))
            {
                typeLabel = "string";
                paramViewModel = new StringParamViewModel(kvp.Value, kvp.Key, model);
            }
            else if (type == typeof(int))
            {
                typeLabel = "int";
                paramViewModel = new IntParamViewModel(kvp.Value, kvp.Key, model, (int)kvp.Value.Min, (int)kvp.Value.Max);
            }
            else if (type == typeof(float))
            {
                typeLabel = "float";
                paramViewModel = new FloatParamViewModel(kvp.Value, kvp.Key, model, kvp.Value.Min, kvp.Value.Max);
            }
            else if (type == typeof(bool))
            {
                return (v,p) => ValidateBool(v,p);
            }
            else if (type == typeof(string[]))
            {
                typeLabel = "string[]";
                paramViewModel = new StrArrayParamViewModel(kvp.Value, kvp.Key, model);
            }
            else if (type.IsEnum)
            {
                typeLabel = "enum";
                paramViewModel = new EnumParamViewModel(kvp.Value, kvp.Key, model);
            }
            else
            {
                throw new NotSupportedException($"Type {type} is not supported.");
            }
        }

        public static string ValidateBool<T>(T b, ParameterAttribute _parameterPromptAttribute)
        {
            return "";
        }

        public static string ValidateEnum<T>(T i, ParameterAttribute _parameterPromptAttribute)
        {
            return "";
        }

        public static string ValidateFloat<T>(T val, ParameterAttribute _parameterPromptAttribute)
        {
            float f = (float)val;
            if (_parameterPromptAttribute.Min != _parameterPromptAttribute.Max)
            {
                if (f < _parameterPromptAttribute.Min)
                {
                    return $"Value must be greater than or equal to {_parameterPromptAttribute.Min}";
                }
                if (f > _parameterPromptAttribute.Max)
                {
                    return $"Value must be less than or equal to {_parameterPromptAttribute.Max}";
                }
            }
            return "";
        }

        public static bool ValidateInt(int i, ParameterAttribute _parameterPromptAttribute, string errorMsg)
        {
            errorMsg = "";
            if (_parameterPromptAttribute.Min != _parameterPromptAttribute.Max)
            {
                if (i < (int)_parameterPromptAttribute.Min)
                {
                    errorMsg = $"Value must be greater than or equal to {(int)_parameterPromptAttribute.Min}";
                    return false;
                }
                if (i > (int)_parameterPromptAttribute.Max)
                {
                    errorMsg = $"Value must be less than or equal to {(int)_parameterPromptAttribute.Max}";
                    return false;
                }
            }
            return true;
        }
    }*/
}
