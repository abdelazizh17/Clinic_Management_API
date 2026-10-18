using Clinic_Management_API.Core.DTOs.AuthDTOs;

namespace Clinic_Management_API.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);

        Task<AuthResponseDto> Login(LoginDto dto);

    }
}
