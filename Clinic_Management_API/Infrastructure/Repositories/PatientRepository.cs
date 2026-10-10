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

        public Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return _context.Patients.AnyAsync(p => p.Phone == phoneNumber);
        }
    }
}
