using Clinic_Management_API.Core.DTOs.PatientDTOs;
using FluentValidation;
using Clinic_Management_API.Core.Validation.Extensions;

namespace Clinic_Management_API.Core.Validation.Patient
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Patient Name is Required")
                .MaximumLength(50).WithMessage("The name cannot exceed 50 characters");

            RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("Date of birth is required.")
            .MustBeValidBirthDate();

            RuleFor(p => p.Phone).MustBeValidPhoneNo();

            RuleFor(p => p.Email).MustBeValidEmail();

            RuleFor(p => p.Gender).GenderValidation();
        }
    }
}
