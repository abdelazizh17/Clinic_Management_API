using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_API.Core.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
