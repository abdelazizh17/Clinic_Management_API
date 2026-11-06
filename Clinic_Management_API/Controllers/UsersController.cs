using Clinic_Management_API.Core.DTOs.UserDTOs;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
            return Ok($"Role '{dto.RoleName}' successfully assigned to {dto.Email}.");
        }

        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
        {
            await _userService.UpdateUserRoleAsync(dto.Email, dto.OldRole, dto.NewRole);
            return Ok($"Role for '{dto.Email}' successfully updated to {dto.NewRole}.");
        }
    }
}
