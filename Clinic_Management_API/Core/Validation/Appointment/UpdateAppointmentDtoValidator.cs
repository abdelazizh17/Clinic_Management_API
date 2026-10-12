using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Appointment
{
    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(a => a.DoctorId)
                .GreaterThan(0).WithMessage("Doctor Id is required")
                .When(a => a.DoctorId != null);

            RuleFor(a => a.PatientId)
                .GreaterThan(0).WithMessage("Patient Id is required")
                .When(a => a.PatientId != null);

            RuleFor(a => a.Notes)
                .MaximumLength(500).WithMessage("The note cannot exceed 500 characters")
                .When(a => a.Notes != null);

            RuleFor(a => a.StartTime)
                .NotEmpty().WithMessage("Start Time is required")
                .LessThan(a => a.EndTime).WithMessage("Start Time must be less than End time")
                .When(a => a.StartTime != null);

            RuleFor(a => a.EndTime)
                            .NotEmpty().WithMessage("End Time is required")
                            .GreaterThan(a => a.StartTime).WithMessage("End Time must be greater than Start time")
                            .When(a => a.EndTime != null);

            RuleFor(a => a.AppointmentDate)
                .NotEmpty().WithMessage("Appointment Date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("This Date is Expired")
                .When(a => a.AppointmentDate != null);
        }
    }
}
