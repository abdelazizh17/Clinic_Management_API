using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.Validation.Extensions;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Doctor
{
    public class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorDtoValidator()
        {
            RuleFor(d => d.Id)
                .GreaterThan(0).WithMessage("Valid Doctor ID is required");

            RuleFor(d => d.FullName)
              .NotEmpty().WithMessage("Doctor Name is Required")
              .MaximumLength(50).WithMessage("The name cannot exceed 50 characters")
              .When(d => d.FullName != null);

            RuleFor(d => d.Phone!)
                .MustBeValidPhoneNo()
                .When(d => !string.IsNullOrEmpty(d.Phone));

            RuleFor(d => d.Email)
                .MustBeValidEmail()
                .When(d => !string.IsNullOrEmpty(d.Email));

            RuleFor(d => d.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.")
                .When(d => d.LicenseNumber != null); 

            RuleFor(d => d.ConsultationFee)
                .GreaterThanOrEqualTo(0).WithMessage("Consultation fee cannot be negative.")
                .LessThanOrEqualTo(10000).WithMessage("Consultation fee exceeds the allowed limit.")
                .PrecisionScale(8, 2, false).WithMessage("Consultation fee must have at most 2 decimal places.")
                .When(d => d.ConsultationFee != null);

            RuleFor(d => d.SpecialtyId)
                .GreaterThan(0).WithMessage("Valid Specialty ID is required")
                .When(d => d.SpecialtyId != null);
        }
    }
}
