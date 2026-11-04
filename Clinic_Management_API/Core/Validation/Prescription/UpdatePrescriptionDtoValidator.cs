using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Prescription
{
    public class UpdatePrescriptionDtoValidator : AbstractValidator<UpdatePrescriptionDto>
    {
        public UpdatePrescriptionDtoValidator()
        {
            RuleFor(p => p.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
                .When(p => p.Notes != null);
        }
    }
}