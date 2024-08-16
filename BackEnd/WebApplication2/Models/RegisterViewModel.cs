using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string SecurityQuestion { get; set; } 
        public string SecurityAnswer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [CustomValidation(typeof(ValidationMethods), nameof(ValidationMethods.ValidateAge))]
        public DateTime DateOfBirth { get; set; } = new DateTime(1996, 7, 29);
    }
    public static class ValidationMethods
    {
        public static ValidationResult ValidateAge(DateTime dateOfBirth, ValidationContext context)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;

            return age >= 18 ? ValidationResult.Success : new ValidationResult("You must be at least 18 years old.");
        }
    }


}