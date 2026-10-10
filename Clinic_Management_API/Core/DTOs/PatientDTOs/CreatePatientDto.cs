using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.PatientDTOs
{
    public class CreatePatientDto
    {
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
