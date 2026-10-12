using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ClinicDBContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateTime dateTime)
        {
            return await _context.Appointments.Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == dateTime).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsAppointmentDateAvailable(DateTime dateTime)
        {
            return await _context.Appointments.AnyAsync(a => a.AppointmentDate == dateTime);

        }
    }
}
