using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_API.Core.Entities
{
    public class PrescriptionItem
    {
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; } = null!;
        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Instructions { get; set; } = null!;
    }
}
