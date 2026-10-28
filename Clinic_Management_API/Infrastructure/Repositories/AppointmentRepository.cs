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

        private IQueryable<Appointment> QueryWithDetails()
        {
            return _context.Appointments
                .Include(A => A.Patient)
                .Include(A => A.Doctor);
        }

        public async Task<IReadOnlyList<Appointment>> GetPagedWithDetailsAsync(Expression<Func<Appointment, bool>>? predicate, int pageNumber, int pageSize)
        {
            IQueryable<Appointment> query = QueryWithDetails();

            return await ApplyPaginationAsync(query, predicate,(Expression<Func<Appointment, int>>?)null, true, pageNumber, pageSize);
        }

        public async Task<Appointment?> GetByIdWithDetails(int id)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyList<Appointment>> GetDoctorAppointmentsAsync(int doctorId, DateOnly dateTime)
        {
            return await QueryWithDetails()
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == dateTime)
                .AsNoTracking()
                .ToListAsync();
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
