using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface ISpecialtyRepository : IGenericRepository<Specialty>
    {
        Task<bool> IsSpecialtyNameExistsAsync(string name);

        Task<Specialty?> GetByIdWithDetailsAsync(int id);

        Task<IReadOnlyList<Specialty>> GetPagedWithSpecAsync(
            Expression<Func<Specialty, bool>>? predicate,
            int pageNumber,
            int pageSize);
    }
}
