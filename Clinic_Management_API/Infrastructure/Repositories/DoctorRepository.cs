using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ClinicDBContext context) : base(context) { }
        public Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return _context.Doctors.AnyAsync(p => p.Phone == phoneNumber);

        }
    }
}
