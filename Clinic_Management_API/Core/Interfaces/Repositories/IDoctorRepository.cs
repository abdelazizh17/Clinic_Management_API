using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        Task<Doctor?> GetByIdWithSpecAsync(int id);
        Task<IReadOnlyList<Doctor>> GetPagedWithSpecAsync(Expression<Func<Doctor, bool>>? predicate, int pageNumber, int pageSize);
        Task<IReadOnlyList<Doctor>> GetPagedWithSpecAsync<TKey>(Expression<Func<Doctor, bool>>? predicate, Expression<Func<Doctor, TKey>>? orderByDescending, bool isAscending, int pageNumber, int pageSize);

    }
}
