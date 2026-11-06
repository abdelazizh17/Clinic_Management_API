namespace Clinic_Management_API.Core.DTOs.AuthDTOs
{
    /// <summary>
    /// The result of creating a JWT. It keeps the token and its expiry together
    /// so callers never have to re-calculate the expiration date.
    /// </summary>
    public record TokenResult(string Token, DateTime ExpiresOnUtc);
}
