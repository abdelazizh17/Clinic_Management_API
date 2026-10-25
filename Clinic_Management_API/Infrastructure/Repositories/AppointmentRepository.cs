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

        public async Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateOnly dateTime)
        {
            return await _context.Appointments.Where(a => a.DoctorId == doctorId && a.AppointmentDate == dateTime).AsNoTracking().ToListAsync();
        }

        public async Task<bool> HasDoctorOverlapAsync(int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Appointments.AnyAsync(a => a.DoctorId == doctorId &&
            a.AppointmentDate == date &&
            a.Status != Core.Enums.AppointmentStatus.Cancelled &&
            startTime < a.EndTime &&
            endTime > a.StartTime);
        }

        public async Task<bool> HasDoctorOverlapForUpdateAsync(int appointmentId, int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Appointments.AnyAsync(a =>
            a.Id != appointmentId &&
            a.DoctorId == doctorId &&
            a.AppointmentDate == date &&
            a.Status != Core.Enums.AppointmentStatus.Cancelled &&
            startTime < a.EndTime &&
            endTime > a.StartTime);
        }
    }
}
