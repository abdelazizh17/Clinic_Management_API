namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task AssignRoleToUserAsync(string email, string roleName);

        public Task UpdateUserRoleAsync(string email, string oldRole, string newRole);
    }
}
