using Clinic_Management_API.Core.DTOs.PatientDTOs;
using FluentValidation;
using Clinic_Management_API.Core.Validation.Extensions;


namespace Clinic_Management_API.Core.Validation.Patient
{
    public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("Valid Patient ID is required.");

            RuleFor(x => x.DateOfBirth!.Value)
            .MustBeValidBirthDate()
            .When(x => x.DateOfBirth.HasValue);

            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Patient Name is Required")
                .MaximumLength(50).WithMessage("The name cannot exceed 50 characters")
                .When(p => p.FullName != null);

            RuleFor(p => p.Phone!)
                .MustBeValidPhoneNo()
                .When(p => !string.IsNullOrEmpty(p.Phone));

            RuleFor(p => p.Email)
                .MustBeValidEmail()
                .When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p.Gender!.Value)
                .GenderValidation()
                .When(p => p.Gender.HasValue);
        }
    }
}
