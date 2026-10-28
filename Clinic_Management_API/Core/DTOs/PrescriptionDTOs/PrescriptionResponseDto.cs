using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;

namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class PrescriptionResponseDto
    {
        public int Id { get; set; }
        public PatientSummaryDto Patient { get; set; } = null!;
        public string DoctorSummaryDto { get; set; } = null!;
        public int AppointmentId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionItemResponseDto> PrescriptionItems { get; set; } = new();
    }
}
