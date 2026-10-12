namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients {  get; }
        IDoctorRepository Doctors { get; }
        IAppointmentRepository Appointments { get; }
        Task<int> CompleteAsync();
    }
}
