using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_API.Core.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();


    }
}
