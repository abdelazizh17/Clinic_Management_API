using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);

    }
}
