using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(ClinicDBContext context) : base(context)
        {
        }

        public async Task<Prescription?> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionItems)
                .ThenInclude(pi => pi.Medication) 
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);
        }
    }
}