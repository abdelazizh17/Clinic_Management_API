using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.SpecialtyDTOs
{
    public class CreateSpecialtyDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
