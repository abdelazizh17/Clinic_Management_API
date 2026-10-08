using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.AppointmentDTOs
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; } = null!;
        public string? Notes { get; set; }
        public string Patient { get; set; } = null!;
        public string Doctor { get; set; } = null!;
    }
}
