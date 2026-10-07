using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_API.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public decimal ConsultationFee { get; set; }
        public bool IsActive { get; set; }
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
