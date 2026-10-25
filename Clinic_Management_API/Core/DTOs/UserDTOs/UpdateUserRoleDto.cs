namespace Clinic_Management_API.Core.DTOs.UserDTOs
{
    public class UpdateUserRoleDto
    {
        public string Email { get; set; } = null!;
        public string OldRole { get; set; } = null!;
        public string NewRole { get; set; } = null!;
    }
}
