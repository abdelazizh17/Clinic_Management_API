using Clinic_Management_API.Core.DTOs.UserDTOs;
using Clinic_Management_API.Core.Validation.Extensions;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.User
{
    public class AssignRoleDtoValidation : AbstractValidator<AssignRoleDto>
    {
        public AssignRoleDtoValidation()
        {
            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email must be required")
                .MustBeValidEmail();

            RuleFor(a => a.RoleName)
                .NotEmpty().WithMessage("Role name must be required")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters");
        }
    }
}
