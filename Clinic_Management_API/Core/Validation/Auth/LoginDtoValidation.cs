using Clinic_Management_API.Core.DTOs.AuthDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Auth
{
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(l => l.EmailOrUsername)
                .NotEmpty().WithMessage("Email or username is required.");

            RuleFor(l => l.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
