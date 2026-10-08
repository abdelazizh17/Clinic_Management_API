using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;

namespace Clinic_Management_API.Core.Interfaces
{
    public interface ISpecialtyService
    {
        Task<SpecialtyResponseDto> CreateAsync(CreateSpecialtyDto dto);
        Task<bool> UpdateAsync(int id, UpdateSpecialtyDto dto);
        Task<bool> DeleteAsync(int id);
        Task<SpecialtyResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<SpecialtyResponseDto>> GetAllAsync(BaseFilterDto filter);
    }
}
