using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.Validation.Extensions;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Doctor
{
    public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorDtoValidator()
        {
            RuleFor(d => d.FullName)
                .NotEmpty().WithMessage("Doctor Name is Required")
                .MaximumLength(50).WithMessage("The name cannot exceed 50 characters");

            RuleFor(d => d.Phone).MustBeValidPhoneNo();

            RuleFor(d => d.Email).MustBeValidEmail();

            RuleFor(d => d.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.");

            RuleFor(d => d.ConsultationFee)
                .GreaterThanOrEqualTo(0).WithMessage("Consultation fee cannot be negative.")
                .LessThanOrEqualTo(10000).WithMessage("Consultation fee exceeds the allowed limit.")
                .PrecisionScale(8, 2, false).WithMessage("Consultation fee must have at most 2 decimal places.");

            RuleFor(d => d.SpecialtyId)
                .GreaterThan(0).WithMessage("Specialty ID is required");
        }
    }
}
