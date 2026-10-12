using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Appointment
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(a => a.DoctorId).GreaterThan(0).WithMessage("Doctor Id is required");

            RuleFor(a=>a.PatientId).GreaterThan(0).WithMessage("Patient Id is required");

            RuleFor(a => a.Notes).MaximumLength(500).WithMessage("The note cannot exceed 500 characters");

            RuleFor(a => a.StartTime)
                .NotEmpty().WithMessage("Start Time is required")
                .LessThan(a => a.EndTime).WithMessage("Start Time must be less than End time");

            RuleFor(a => a.EndTime)
                            .NotEmpty().WithMessage("End Time is required")
                            .GreaterThan(a => a.StartTime).WithMessage("End Time must be greater than Start time");

            RuleFor(a => a.AppointmentDate)
                .NotEmpty().WithMessage("Appointment Date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("This Date is Expired");
        }
    }
}
