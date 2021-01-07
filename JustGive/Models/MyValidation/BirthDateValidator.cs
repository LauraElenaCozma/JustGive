using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustGive.Models.MyValidation
{
    public class BirthDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contactInfo = (Models.ContactInfo)validationContext.ObjectInstance;
            DateTime birthDate = contactInfo.BirthDate;
            int maximumAge = 117;       //the maximum age that a person can have
            var currentDate = DateTime.Now;
            var startDate = currentDate.AddYears(-maximumAge);
            if (birthDate <= startDate)
            {
                return new ValidationResult
                    ("Birth date cannot be less than 117 years ago.");
            }
            else if (birthDate >= currentDate)
            {
                return new ValidationResult
                    ("Birth date cannot be greater or equal than current date.");
            }
            return ValidationResult.Success;
        }
    }
}