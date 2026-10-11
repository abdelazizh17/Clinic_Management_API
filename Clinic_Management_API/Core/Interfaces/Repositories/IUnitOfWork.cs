namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients {  get; }
        IDoctorRepository Doctors { get; }
        Task<int> CompleteAsync();
    }
}
