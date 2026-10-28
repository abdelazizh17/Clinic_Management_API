using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.AppointmentDTOs
{
    public class UpdateAppointmentDto
    {
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public AppointmentStatus? Status { get; set; }
        public string? Notes { get; set; }
        public int? PatientId { get; set; }
        public int? DoctorId { get; set; }
    }
}
