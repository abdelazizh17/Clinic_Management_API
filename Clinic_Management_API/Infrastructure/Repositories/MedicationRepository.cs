using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class MedicationRepository : GenericRepository<Medication>, IMedicationRepository
    {
        public MedicationRepository(ClinicDBContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Medication>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Medications.Select(m => m).ToListAsync();
        }

        public async Task<bool> IsMedicationNameExistAsync(string name)
        {
            return await _context.Medications.AnyAsync(m => m.Name == name);
        }
    }
}
