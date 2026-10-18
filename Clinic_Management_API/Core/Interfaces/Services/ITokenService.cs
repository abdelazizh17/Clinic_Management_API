using Clinic_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}