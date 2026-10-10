using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.AppointmentDTOs;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
        Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task<bool> DeleteAsync(int id);
        Task<AppointmentResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<AppointmentResponseDto>> GetAllAsync(BaseFilterDto filter);
        Task<IReadOnlyList<AppointmentResponseDto>> GetDoctorAppointmentsAsync(int doctorId, DateTime dateTime);
        Task<bool> ChangeStatusAsync(int AppointmentId,int Status);
    }
}
