using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDBContext _context;
        public IPatientRepository Patients { get; private set; }

        public UnitOfWork(ClinicDBContext context)
        {
            _context = context;
            Patients = new PatientRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
