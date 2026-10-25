namespace Clinic_Management_API.Core.DTOs.AuthDTOs
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } 
        public string Token { get; set; }
        public string ExpiresOn { get; set; }
        public List<string> Roles { get; set; }

    }
}
