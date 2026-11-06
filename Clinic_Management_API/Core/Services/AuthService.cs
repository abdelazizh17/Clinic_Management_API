using Clinic_Management_API.Core.DTOs.AuthDTOs;
using Clinic_Management_API.Core.Exceptions;
using Clinic_Management_API.Core.Interfaces.Services;
using Clinic_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Clinic_Management_API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = dto.EmailOrUsername.Contains("@")
                ? await _userManager.FindByEmailAsync(dto.EmailOrUsername)
                : await _userManager.FindByNameAsync(dto.EmailOrUsername);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Invalid email/username or password."
                };
            }

            var tokenResult = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "Logged in successfully.",
                Token = tokenResult.Token,
                ExpiresOn = tokenResult.ExpiresOnUtc,
                Roles = roles.ToList()
            };
        }

        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.Phone
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            var tokenResult = await _tokenService.CreateTokenAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "User registered successfully. Assign a role to the user before they can access protected endpoints.",
                Token = tokenResult.Token,
                ExpiresOn = tokenResult.ExpiresOnUtc,
                Roles = new List<string>()
            };
        }
    }
}
