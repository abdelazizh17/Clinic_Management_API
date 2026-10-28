using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class SpecialtyRepository
        : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(ClinicDBContext context)
            : base(context)
        {
        }

        private IQueryable<Specialty> QueryWithDetails()
        {
            return _context.Specialties
                .Include(s => s.Doctors)
                .AsSplitQuery();
        }

        public async Task<Specialty?> GetByIdWithDetailsAsync(int id)
        {
            return await QueryWithDetails().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IReadOnlyList<Specialty>> GetPagedWithSpecAsync(Expression<Func<Specialty, bool>>? predicate, int pageNumber, int pageSize)
        {
            IQueryable<Specialty> query = QueryWithDetails();

            return await ApplyPaginationAsync(
                query,
                predicate,
                (Expression<Func<Specialty, int>>?)null,
                true,
                pageNumber,
                pageSize);
        }

        public async Task<bool> IsSpecialtyNameExistsAsync(string name)
        {
            return await _context.Specialties.AnyAsync(s => s.Name.ToLower() == name.Trim().ToLower());
        }
    }
}