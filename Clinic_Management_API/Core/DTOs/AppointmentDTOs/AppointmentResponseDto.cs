
namespace Clinic_Management_API.Core.DTOs.AppointmentDTOs
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
        public PatientSummaryDto Patient { get; set; } = null!;
        public DoctorSummaryDto Doctor { get; set; } = null!;
    }
}
