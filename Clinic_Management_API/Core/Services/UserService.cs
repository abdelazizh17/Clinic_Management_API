using Clinic_Management_API.Core.Exceptions;
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

            if (user is null)
            {
                _logger.LogWarning("User with email {Email} was not found", email);
                throw new NotFoundException($"User with email '{email}' was not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                _logger.LogWarning(
                    "Failed to add role {RoleName} to user {Email}: {Errors}",
                    roleName,
                    email,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                throw new BadRequestException($"Failed to add role '{roleName}' to the user.");
            }
        }

        public async Task UpdateUserRoleAsync(string email, string oldRole, string newRole)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogWarning("User with email {Email} was not found", email);
                throw new NotFoundException($"User with email '{email}' was not found.");
            }

            var isInOldRole = await _userManager.IsInRoleAsync(user, oldRole);

            if (!isInOldRole)
            {
                _logger.LogWarning("User {Email} does not have the role {OldRole}", email, oldRole);
                throw new BadRequestException($"User with email '{email}' does not currently have the role '{oldRole}'.");
            }

            var removeResult = await _userManager.RemoveFromRoleAsync(user, oldRole);

            if (!removeResult.Succeeded)
            {
                _logger.LogWarning("Failed to remove role {OldRole} from user {Email}", oldRole, email);
                throw new BadRequestException($"Failed to remove the existing role '{oldRole}'.");
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);

            if (!addResult.Succeeded)
            {
                _logger.LogWarning("Failed to add role {NewRole} to user {Email}", newRole, email);
                throw new BadRequestException($"Failed to assign the new role '{newRole}'.");
            }
        }
    }
}
