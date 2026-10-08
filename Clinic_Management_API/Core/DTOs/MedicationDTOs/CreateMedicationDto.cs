using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.DTOs.MedicationDTOs
{
    public class CreateMedicationDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

    }
}
