using Clinic_Management_API.Core.DTOs.UserDTOs;
using Clinic_Management_API.Core.Validation.Extensions;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.User
{
    public class UpdateUserRoleDtoValidation : AbstractValidator<UpdateUserRoleDto>
    {
        public UpdateUserRoleDtoValidation()
        {
            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email must be required")
                .MustBeValidEmail();

            RuleFor(a => a.NewRole)
                .NotEmpty().WithMessage("New role must be required")
                .MaximumLength(50).WithMessage("New role cannot exceed 50 characters");

            RuleFor(a => a.OldRole)
                .NotEmpty().WithMessage("Old role must be required")
                .MaximumLength(50).WithMessage("Old role cannot exceed 50 characters");
        }
    }
}
