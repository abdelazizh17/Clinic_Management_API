using Clinic_Management_API.Core.DTOs.AuthDTOs;
using Clinic_Management_API.Core.Validation.Extensions;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Auth
{
    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must be required")
                .MustBeValidEmail();

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone must be required")
                .MustBeValidPhoneNo();

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName must be required")
                .MinimumLength(8).WithMessage("Username at least has 8 characters");

        }
    }
}
