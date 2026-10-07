using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
    {
        public void Configure(EntityTypeBuilder<Medication> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(50);

            builder.Property(m => m.Description).HasMaxLength(200);

            builder.Property(m => m.Price).HasPrecision(10,2);

        }
    }
}
