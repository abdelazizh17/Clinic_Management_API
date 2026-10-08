namespace Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs
{
    public class CreatePrescriptionItemDto
    {
        public int MedicationId { get; set; }
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Instructions { get; set; } = null!;
    }
}
