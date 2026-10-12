using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Specialty
{
    public class UpdateSpecialtyDtoValidator : AbstractValidator<UpdateSpecialtyDto>
    {
        public UpdateSpecialtyDtoValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Specialty name is required.")
                .MaximumLength(100).WithMessage("Specialty name cannot exceed 100 characters.")
                .When(s => s.Name != null);

            RuleFor(s => s.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .When(s => s.Description != null);
        }
    }
}