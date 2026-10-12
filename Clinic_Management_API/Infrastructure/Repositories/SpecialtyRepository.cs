using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(ClinicDBContext context) : base(context)
        {
        }

        public async Task<bool> IsSpecialtyNameExistsAsync(string name)
        {
            return await _context.Specialties.AnyAsync(s => s.Name.ToLower() == name.Trim().ToLower());
        }
    }
}