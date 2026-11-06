using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PatientResponseDto> CreateAsync(CreatePatientDto createPatientDto);
        Task<bool> UpdateAsync(int id, UpdatePatientDto updatePatientDto);
        Task<PatientResponseDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<PatientResponseDto>> GetPatientsWithMultipleAppointments(PaginationDto filter);
        Task<PagedResult<PatientResponseDto>> GetAllAsync(BaseFilterDto filter);
    }
}
