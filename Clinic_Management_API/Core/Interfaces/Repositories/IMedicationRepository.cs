using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Interfaces.Repositories
{
    public interface IMedicationRepository : IGenericRepository<Medication>
    {
        Task<bool> IsMedicationNameExistAsync(string name);
    }
}
