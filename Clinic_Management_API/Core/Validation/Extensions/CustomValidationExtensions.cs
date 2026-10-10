using Clinic_Management_API.Core.Enums;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Extensions
{
    public static class CustomValidationExtensions
    {
        public static IRuleBuilderOptions<T,string?> MustBeValidEmail<T>(this IRuleBuilder<T,string?> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("Incorrect email format (e.g., example@domain.com)");
        }

        public static IRuleBuilderOptions<T,string> MustBeValidPhoneNo<T>(this IRuleBuilder<T,string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Phone No. is Required")
                .Matches(@"^01[0125]\d{8}$").WithMessage("Invalid mobile number. It must be an 11-digit Egyptian phone number");
        }

        public static IRuleBuilderOptions<T, Gender> GenderValidation<T>(this IRuleBuilder<T, Gender> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum().WithMessage("Please select a valid gender.");
        }


        public static IRuleBuilderOptions<T, DateTime> MustBeValidBirthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Invalid date of birth.");
        }
    }
}
