using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        Task<Prescription?> GetByAppointmentIdAsync(int appointmentId);
        Task<Prescription?> GetByIdWithDetailsAsync(int id);
        Task<IReadOnlyList<Prescription>> GetPagedWithSpecAsync(
            Expression<Func<Prescription, bool>>? predicate,
            int pageNumber,
            int pageSize);
    }
}