using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Specialty
{
    public class CreateSpecialtyDtoValidator : AbstractValidator<CreateSpecialtyDto>
    {
        public CreateSpecialtyDtoValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Specialty name is required.")
                .MaximumLength(100).WithMessage("Specialty name cannot exceed 100 characters.");

            RuleFor(s => s.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .When(s => s.Description != null);
        }
    }
}