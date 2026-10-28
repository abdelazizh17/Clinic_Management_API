using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class PrescriptionRepository
        : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(ClinicDBContext context)
            : base(context)
        {
        }

        private IQueryable<Prescription> QueryWithDetails()
        {
            return _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionItems)
                    .ThenInclude(pi => pi.Medication);
        }

        public async Task<Prescription?> GetByIdWithDetailsAsync(int id)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Prescription?> GetByAppointmentIdAsync(
            int appointmentId)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }

        public async Task<IReadOnlyList<Prescription>> GetPagedWithSpecAsync(
            Expression<Func<Prescription, bool>>? predicate,
            int pageNumber,
            int pageSize)
        {
            IQueryable<Prescription> query = QueryWithDetails();

            return await ApplyPaginationAsync(
                query,
                predicate,
                (Expression<Func<Prescription, int>>?)null,
                true,
                pageNumber,
                pageSize);
        }
    }
}