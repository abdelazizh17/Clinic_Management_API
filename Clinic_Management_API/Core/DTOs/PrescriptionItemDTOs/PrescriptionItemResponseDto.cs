using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs
{
    public class PrescriptionItemResponseDto
    {
        public string MedicationName { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Instructions { get; set; } = null!;
    }
}
