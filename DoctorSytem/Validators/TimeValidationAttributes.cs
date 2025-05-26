using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Validators
{
    public class TimeRangeAttribute : ValidationAttribute
    {
        private readonly int _startHour;
        private readonly int _endHour;

        public TimeRangeAttribute(int startHour, int endHour)
        {
            _startHour = startHour;
            _endHour = endHour;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is TimeSpan time)
            {
                if (time.Hours < _startHour || time.Hours >= _endHour)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    public class TimeAfterAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public TimeAfterAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property {_otherPropertyName}");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
            if (value is TimeSpan time && otherValue is TimeSpan otherTime)
            {
                if (time <= otherTime)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
} 