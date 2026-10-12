using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime dateTime);

        Task<bool> IsAppointmentDateAvailable(DateTime dateTime);

    }
}
