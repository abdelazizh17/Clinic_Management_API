using Clinic_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Management_API.Core.Interfaces.Services
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
    }
}
