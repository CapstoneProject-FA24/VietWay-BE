using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VietWay.API.Management.RequestModel.CustomValidation
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object[] _expectedValues;

        public RequiredIfAttribute(string propertyName, params object[] expectedValues)
        {
            _propertyName = propertyName;
            _expectedValues = expectedValues;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            PropertyInfo? property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
            {
                return new ValidationResult($"Unknown property {_propertyName}");
            }
            var propertyValue = property.GetValue(validationContext.ObjectInstance);
            if (_expectedValues.Contains(propertyValue))
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
