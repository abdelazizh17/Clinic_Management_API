namespace Clinic_Management_API.Core.DTOs.AuthDTOs
{
    public class LoginDto
    {
        public string EmailOrUsername { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
