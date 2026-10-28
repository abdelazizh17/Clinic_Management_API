using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(ClinicDBContext context) : base(context)
        {
        }
        private IQueryable<Patient> QueryWithDetails()
        {
            return _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Prescriptions)
                .AsSplitQuery();
        }
        public async Task<Patient?> GetByIdWithDetailsAsync(int id)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Patient>> GetPagedWithSpecAsync(Expression<Func<Patient, bool>>? predicate, int pageNumber, int pageSize)
        {
            IQueryable<Patient> query = QueryWithDetails();

            return await ApplyPaginationAsync(query, predicate, (Expression<Func<Patient, int>>?)null, true, pageNumber, pageSize);
        }

        public Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return _context.Patients.AnyAsync(p => p.Phone == phoneNumber);
        }
    }
}
