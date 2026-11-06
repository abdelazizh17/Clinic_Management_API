using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Appointment
{
    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(a => a.DoctorId)
                .GreaterThan(0).WithMessage("Doctor Id is required.")
                .When(a => a.DoctorId != null);

            RuleFor(a => a.PatientId)
                .GreaterThan(0).WithMessage("Patient Id is required.")
                .When(a => a.PatientId != null);

            RuleFor(a => a.Notes)
                .MaximumLength(500).WithMessage("The note cannot exceed 500 characters.")
                .When(a => a.Notes != null);

            RuleFor(a => a.StartTime)
                .Must(t => t != default).WithMessage("Start Time is required.")
                .LessThan(a => a.EndTime).WithMessage("Start Time must be less than End Time.")
                .When(a => a.StartTime != null);

            RuleFor(a => a.EndTime)
                .Must(t => t != default).WithMessage("End Time is required.")
                .GreaterThan(a => a.StartTime).WithMessage("End Time must be greater than Start Time.")
                .When(a => a.EndTime != null);

            RuleFor(a => a.AppointmentDate)
                .Must(d => d != default).WithMessage("Appointment Date is required.")
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Appointment date cannot be in the past.")
                .When(a => a.AppointmentDate != null);
        }
    }
}
