using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.MedicationDTOs;
namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IMedicationService
    {
        Task<MedicationResponseDto> CreateAsync(CreateMedicationDto dto);
        Task<bool> UpdateAsync(int id, UpdateMedicationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<MedicationResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<MedicationResponseDto>> GetAllAsync(BaseFilterDto filter);
    }
}
