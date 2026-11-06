namespace Clinic_Management_API.Core.Options
{
    public class SeedOptions
    {
        public const string SectionName = "Seed";

        public string AdminEmail { get; set; } = string.Empty;
        public string AdminUserName { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
    }
}
