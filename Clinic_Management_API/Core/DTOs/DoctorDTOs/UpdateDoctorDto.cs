namespace Clinic_Management_API.Core.DTOs.DoctorDTOs
{
    public class UpdateDoctorDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? LicenseNumber { get; set; }
        public decimal? ConsultationFee { get; set; }
        public bool? IsActive { get; set; }
        public int? SpecialtyId { get; set; }
    }
}
