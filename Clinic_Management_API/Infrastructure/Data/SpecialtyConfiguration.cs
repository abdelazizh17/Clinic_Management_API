using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.Name).IsUnique();

            builder.Property(s => s.Name).HasMaxLength(100);

            builder.Property(s => s.Description).HasMaxLength(500);
        }
    }
}
