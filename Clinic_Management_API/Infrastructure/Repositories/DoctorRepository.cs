using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ClinicDBContext context) : base(context) { }

        public async Task<IReadOnlyList<Doctor>> GetPagedWithSpecAsync(Expression<Func<Doctor, bool>>? predicate, int pageNumber, int pageSize)
        {
            IQueryable<Doctor> query = _context.Doctors.Include(d => d.Specialty);

            return await ApplyPaginationAsync(query, predicate,(Expression<Func<Doctor, int>>?)null, true, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<Doctor>> GetPagedWithSpecAsync<TKey>(Expression<Func<Doctor, bool>>? predicate, Expression<Func<Doctor, TKey>>? orderByDescending, bool isAscending, int pageNumber, int pageSize)
        {
            IQueryable<Doctor> query = _context.Doctors.Include(d => d.Specialty);

            return await ApplyPaginationAsync(query, predicate, orderByDescending, isAscending, pageNumber, pageSize);
        }

        public async Task<Doctor?> GetByIdWithSpecAsync(int id)
        {
            return await _context.Doctors
            .Include(d => d.Specialty) 
            .FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return _context.Doctors.AnyAsync(p => p.Phone == phoneNumber);

        }
    }
}
