using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.DoctorDTOs
{
    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public string Specialty { get; set; } = null!;

    }
}
