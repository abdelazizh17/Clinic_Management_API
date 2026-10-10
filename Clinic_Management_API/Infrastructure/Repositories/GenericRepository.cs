using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;
using System.Linq.Expressions;

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
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.
                Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public void Update(T entity) => _context.Set<T>().Update(entity);
       
    }
}
