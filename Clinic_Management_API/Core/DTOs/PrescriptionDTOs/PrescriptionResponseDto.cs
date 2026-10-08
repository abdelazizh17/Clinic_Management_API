using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;

namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class PrescriptionResponseDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public int AppointmentId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionItemResponseDto> PrescriptionItems { get; set; } = new();
    }
}
