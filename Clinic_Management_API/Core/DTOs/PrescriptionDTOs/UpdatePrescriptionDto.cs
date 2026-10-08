using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class UpdatePrescriptionDto
    {
        public int Id { get; set; }
        public string? Notes { get; set; }
        public List<UpdatePrescriptionItemDto> PrescriptionItems { get; set; } = new();
    }
}
