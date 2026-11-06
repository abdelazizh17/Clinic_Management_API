using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<DoctorResponseDto?> GetByIdAsync(int id);
        Task<PagedResult<DoctorResponseDto>> GetAllAsync(BaseFilterDto filter);
        Task<PagedResult<DoctorResponseDto>> GetDoctorsWithMostAppointments(BaseFilterDto filter, bool isAscending);
        Task<PagedResult<DoctorResponseDto>> GetDoctorsBySpecialtyId(int specialtyId, PaginationDto filter);
    }
}
