namespace Clinic_Management_API.Core.Exceptions
{
    /// <summary>
    /// Thrown when a request conflicts with the current state of the resource,
    /// for example a duplicate phone number. Mapped to HTTP 409.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
