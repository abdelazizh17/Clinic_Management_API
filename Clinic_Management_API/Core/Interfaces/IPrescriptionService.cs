using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;

namespace Clinic_Management_API.Core.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionDto dto);
        Task<bool> UpdateAsync(int id, UpdatePrescriptionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PrescriptionResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<PrescriptionResponseDto>> GetAllAsync(BaseFilterDto filter);
        Task<PrescriptionResponseDto> GetByAppointmentIdAsync(int AppointmentId);
    }
}
