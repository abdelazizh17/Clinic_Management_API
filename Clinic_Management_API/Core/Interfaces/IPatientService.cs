using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;

namespace Clinic_Management_API.Core.Interfaces
{
    public interface IPatientService
    {
        Task<PatientResponseDto> CreateAsync(CreatePatientDto createPatientDto);
        Task<bool> UpdateAsync(int id,UpdatePatientDto updatePatientDto);
        Task<PatientResponseDto?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyList<PatientResponseDto>> GetAllAsync(BaseFilterDto filter);
    }
}
