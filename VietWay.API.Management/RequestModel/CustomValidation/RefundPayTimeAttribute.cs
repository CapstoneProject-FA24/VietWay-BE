﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VietWay.API.Management.RequestModel.CustomValidation
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a date and time in the past.";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
#warning warning: Datetime inconsitent, check later
                if (dateTime < TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh")))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"{validationContext.DisplayName} must be a date and time in the past.");
            }
            return new ValidationResult($"{validationContext.DisplayName} is not a valid date and time.");
        }
    }
}
