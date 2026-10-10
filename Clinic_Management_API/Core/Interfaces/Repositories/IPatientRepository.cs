using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
    }
}
