using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Prescription
{
    public class CreatePrescriptionDtoValidator : AbstractValidator<CreatePrescriptionDto>
    {
        public CreatePrescriptionDtoValidator()
        {
            RuleFor(p => p.PatientId)
                .GreaterThan(0).WithMessage("Patient Id is required.");

            RuleFor(p => p.DoctorId)
                .GreaterThan(0).WithMessage("Doctor Id is required.");

            RuleFor(p => p.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment Id is required.");

            RuleFor(p => p.PrescriptionDate)
                .NotEmpty().WithMessage("Prescription Date is required.");

            RuleFor(p => p.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

            RuleFor(p => p.PrescriptionItems)
                .NotEmpty().WithMessage("Prescription must contain at least one item.");
        }
    }
}