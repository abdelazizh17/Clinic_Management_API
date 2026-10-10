using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.DoctorDTOs;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<DoctorResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<DoctorResponseDto>> GetAllAsync(BaseFilterDto filter);
        Task<IReadOnlyList<DoctorResponseDto>> GetDoctorsBySpecialtyId(int  specialtyId);
    }
}
