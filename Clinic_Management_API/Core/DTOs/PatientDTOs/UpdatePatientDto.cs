using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.PatientDTOs
{
    public class UpdatePatientDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? Phone { get; set; } 
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
