namespace Clinic_Management_API.Core.DTOs.PrescriptionDTOs
{
    public class PrescriptionSummaryDto
    {
        public int Id { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string DoctorName { get; set; } = null!; 
        public int ItemsCount { get; set; }
    }
}
