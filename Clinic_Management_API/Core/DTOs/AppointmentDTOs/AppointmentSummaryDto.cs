namespace Clinic_Management_API.Core.DTOs.AppointmentDTOs
{
    public class AppointmentSummaryDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string DoctorName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
