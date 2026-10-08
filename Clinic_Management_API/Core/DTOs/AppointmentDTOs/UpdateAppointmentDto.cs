using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.AppointmentDTOs
{
    public class UpdateAppointmentDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
