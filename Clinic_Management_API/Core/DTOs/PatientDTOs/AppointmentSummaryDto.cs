namespace Clinic_Management_API.Core.DTOs.PatientDTOs
{
    public class AppointmentSummaryDto
    {
        public int Id { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string DoctorName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
