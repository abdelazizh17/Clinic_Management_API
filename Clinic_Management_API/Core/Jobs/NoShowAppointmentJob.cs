using Clinic_Management_API.Core.Enums;
using Clinic_Management_API.Core.Interfaces.Repositories;

namespace Clinic_Management_API.Core.Jobs
{
    public class NoShowAppointmentJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NoShowAppointmentJob> _logger;

        public NoShowAppointmentJob(
            IUnitOfWork unitOfWork,
            ILogger<NoShowAppointmentJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task MarkNoShowAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var appointments = await _unitOfWork.Appointments.GetScheduledBeforeAsync(today);

            var count = 0;

            foreach (var appointment in appointments)
            {
                var end = appointment.AppointmentDate.ToDateTime(appointment.EndTime);

                if (end >= DateTime.Now)
                {
                    continue;
                }

                appointment.Status = AppointmentStatus.NoShow;
                count++;
            }

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("{Count} appointments were marked as NoShow", count);
        }
    }
}
