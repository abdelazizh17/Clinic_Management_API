using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IReadOnlyList<Appointment>> GetPagedWithDetailsAsync(Expression<Func<Appointment, bool>>? predicate, int pageNumber, int pageSize);
        Task<Appointment?> GetByIdWithDetails(int  id);
        Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateOnly dateTime);

        Task<bool> HasDoctorOverlapAsync(int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime);

        Task<bool> HasDoctorOverlapForUpdateAsync(int appointmentId,int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime);


    }
}
