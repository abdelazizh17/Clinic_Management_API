namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients {  get; }
        IDoctorRepository Doctors { get; }
        IAppointmentRepository Appointments { get; }
        IMedicationRepository Medications { get; }
        IPrescriptionRepository Prescriptions { get; }
        ISpecialtyRepository Specialties { get; }

        Task<int> CompleteAsync();
    }
}
