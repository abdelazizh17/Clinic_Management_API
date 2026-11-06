using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ClinicDBContext _context;

        public GenericRepository(ClinicDBContext context) => _context = context;

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync() =>
            await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate,
            int pageNumber,
            int pageSize)
        {
            return await ApplyPaginationAsync(
                _context.Set<T>(),
                predicate,
                (Expression<Func<T, int>>?)null,
                true,
                pageNumber,
                pageSize);
        }

        public async Task<IReadOnlyList<T>> GetPagedAsync<TKey>(
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TKey>>? orderBy,
            bool isAscending,
            int pageNumber,
            int pageSize)
        {
            return await ApplyPaginationAsync(
                _context.Set<T>(),
                predicate,
                orderBy,
                isAscending,
                pageNumber,
                pageSize);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        protected async Task<IReadOnlyList<T>> ApplyPaginationAsync<TKey>(
            IQueryable<T> query,
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TKey>>? orderBy,
            bool isAscending,
            int pageNumber,
            int pageSize)
        {
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            // SQL Server requires ORDER BY when using OFFSET/FETCH.
            // If the caller did not specify an order, order by the primary key
            // so pagination stays stable between requests.
            IOrderedQueryable<T> orderedQuery = orderBy is not null
                ? (isAscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy))
                : query.OrderBy(x => EF.Property<int>(x, "Id"));

            query = orderedQuery.ThenBy(x => EF.Property<int>(x, "Id"));

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
