using Clinic_Management_API.Core.DTOs.AuthDTOs;
using Clinic_Management_API.Infrastructure.Data;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenResult> CreateTokenAsync(ApplicationUser user);
    }
}
