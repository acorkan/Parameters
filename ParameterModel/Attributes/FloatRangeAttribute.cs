using System.ComponentModel.DataAnnotations;

namespace ParameterModel.Attributes
{
    //public class FloatRangeAttribute : ValidationAttribute
    //{
    //    private readonly float _min;
    //    private readonly float _max;
    //    private const float Tolerance = 0.00001f; // Adjust as needed

    //    public FloatRangeAttribute(float min, float max)
    //    {
    //        _min = min;
    //        _max = max;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value is float floatValue)
    //        {
    //            if (floatValue >= _min - Tolerance && floatValue <= _max + Tolerance)
    //            {
    //                return ValidationResult.Success;
    //            }
    //        }
    //        return new ValidationResult(ErrorMessage ?? $"The field {validationContext.DisplayName} must be between {_min} and {_max}.");
    //    }
    //}
}
