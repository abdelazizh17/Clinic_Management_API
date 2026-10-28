using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;

namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class UpdatePrescriptionDto
    {
        public string? Notes { get; set; }
        public List<UpdatePrescriptionItemDto>? PrescriptionItems { get; set; }
    }
}
