namespace Clinic_Management_API.Core.Exceptions
{
    /// <summary>
    /// Thrown when the request is valid from a syntax point of view but violates
    /// a business rule, for example booking with an inactive doctor.
    /// Mapped to HTTP 400.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
