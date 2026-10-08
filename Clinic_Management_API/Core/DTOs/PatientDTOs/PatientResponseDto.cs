using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;

namespace Clinic_Management_API.Core.DTOs.PatientDTOs
{
    public class PatientResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<PrescriptionSummaryDto> Prescriptions { get; set; } = new();
        public List<AppointmentSummaryDto> Appointments { get; set; } = new();
    }
}
