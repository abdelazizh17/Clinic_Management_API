namespace Clinic_Management_API.Core.DTOs.FilterDTOs
{
    public class AppointmentFilterDto : BaseFilterDto
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? AppointmentDate { get; set; }

    }
}
