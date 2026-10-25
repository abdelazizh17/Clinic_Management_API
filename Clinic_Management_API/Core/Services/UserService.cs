using Clinic_Management_API.Core.Interfaces.Services;
using Clinic_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Management_API.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task AssignRoleToUserAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email: {email} not found");
                throw new KeyNotFoundException($"User with email: {email} not found");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Failed to add role to user");
                throw new InvalidOperationException("Failed to add role to user.");
            }
        }

        public async Task UpdateUserRoleAsync(string email, string oldRole, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email: {email} not found.");
                throw new KeyNotFoundException($"User with email '{email}' was not found.");
            }

            var isInOldRole = await _userManager.IsInRoleAsync(user, oldRole);
            if (!isInOldRole)
            {
                _logger.LogWarning($"User with email: {email} does not have the role '{oldRole}'.");
                throw new InvalidOperationException($"User with email '{email}' does not currently have the role '{oldRole}'.");
            }

            var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRole);
            if (!removeResult.Succeeded)
            {
                _logger.LogWarning($"Failed to remove old role '{oldRole}' from user: {email}.");
                throw new InvalidOperationException($"Failed to remove the existing role '{oldRole}'.");
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                _logger.LogWarning($"Failed to add new role '{newRole}' to user: {email}.");
                throw new InvalidOperationException($"Failed to assign the new role '{newRole}'.");
            }
        }
    }
}
