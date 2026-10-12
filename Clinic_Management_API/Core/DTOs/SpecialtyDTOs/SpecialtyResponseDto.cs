using Clinic_Management_API.Core.DTOs.DoctorDTOs;

namespace Clinic_Management_API.Core.DTOs.SpecialtyDTOs
{
    public class SpecialtyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<DoctorResponseDto> Doctors { get; set; } = new();
    }
}
