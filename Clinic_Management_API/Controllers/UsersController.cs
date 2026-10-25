using Clinic_Management_API.Core.DTOs.UserDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
        {
            await _userService.AssignRoleToUserAsync(dto.Email, dto.RoleName);
            return Ok($"Role '{dto.Email}' successfully assigned to {dto.RoleName}.");
        }

        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
        {
            await _userService.UpdateUserRoleAsync(dto.Email, dto.OldRole, dto.NewRole);
            return Ok($"Role '{dto.Email}' successfully Updated to {dto.NewRole}.");
        }
    }
}
