using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        Task<Patient?> GetByIdWithDetailsAsync(int id);
        Task<IReadOnlyList<Patient>> GetPagedWithSpecAsync(Expression<Func<Patient, bool>>? predicate, int pageNumber, int pageSize);
    }
}
