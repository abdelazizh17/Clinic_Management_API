namespace Clinic_Management_API.Core.DTOs.MedicationDTOs
{
    public class MedicationResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
