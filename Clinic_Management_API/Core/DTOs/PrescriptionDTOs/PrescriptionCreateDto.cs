using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class PrescriptionCreateDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public List<CreatePrescriptionItemDto> PrescriptionItems { get; set; } = new();
    }
}
