using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;
namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ClinicDBContext _context;
        public GenericRepository(ClinicDBContext context) => _context = context;

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity) =>  _context.Set<T>().Remove(entity);
      
        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetPagedAsync(Expression<Func<T, bool>>? predicate, int pageNumber, int pageSize)
        {
            return await ApplyPaginationAsync(_context.Set<T>(), predicate, (Expression<Func<T, int>>?)null, true, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<T>> GetPagedAsync<TKey>(Expression<Func<T, bool>>? predicate, Expression<Func<T, TKey>>? orderByDescending, bool isAscending, int pageNumber, int pageSize)
        {
            return await ApplyPaginationAsync(_context.Set<T>(), predicate, orderByDescending, isAscending, pageNumber, pageSize);
        }

        protected async Task<IReadOnlyList<T>> ApplyPaginationAsync<TKey>
            (IQueryable<T> query,
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TKey>>? orderByDescending,
            bool isAscending,
            int pageNumber,
            int pageSize)
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderByDescending != null)
            {
                if (isAscending)
                {
                    query = query.OrderBy(orderByDescending);
                }
                else
                {
                    query = query.OrderByDescending(orderByDescending);
                }
            }

            return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }

        public void Update(T entity) => _context.Set<T>().Update(entity);
       
    }
}
