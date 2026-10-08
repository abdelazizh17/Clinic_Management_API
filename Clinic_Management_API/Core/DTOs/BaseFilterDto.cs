namespace Clinic_Management_API.Core.DTOs
{
    public class BaseFilterDto
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
