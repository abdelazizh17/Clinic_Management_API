namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate,
            int pageNumber,
            int pageSize);

        Task<IReadOnlyList<T>> GetPagedAsync<TKey>(
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TKey>>? orderByDescending,
            bool isAscending,
            int pageNumber,
            int pageSize);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
