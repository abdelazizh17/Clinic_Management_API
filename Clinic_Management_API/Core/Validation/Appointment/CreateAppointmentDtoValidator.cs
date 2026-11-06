using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Appointment
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(a => a.DoctorId)
                .GreaterThan(0).WithMessage("Doctor Id is required.");

            RuleFor(a => a.PatientId)
                .GreaterThan(0).WithMessage("Patient Id is required.");

            RuleFor(a => a.Notes)
                .MaximumLength(500).WithMessage("The note cannot exceed 500 characters.");

            RuleFor(a => a.StartTime)
                .Must(t => t != default).WithMessage("Start Time is required.")
                .LessThan(a => a.EndTime).WithMessage("Start Time must be less than End Time.");

            RuleFor(a => a.EndTime)
                .Must(t => t != default).WithMessage("End Time is required.")
                .GreaterThan(a => a.StartTime).WithMessage("End Time must be greater than Start Time.");

            RuleFor(a => a.AppointmentDate)
                .Must(d => d != default).WithMessage("Appointment Date is required.")
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Appointment date cannot be in the past.");
        }
    }
}
