using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FullName).HasMaxLength(50);
            builder.Property(p => p.Phone).HasMaxLength(30);
            builder.Property(p => p.Address).HasMaxLength(50);
            builder.Property(p => p.Email).HasMaxLength(320);

            builder.Property(p => p.DateOfBirth).HasColumnType("date");

            builder.Property(p => p.Gender)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(p => p.Phone).IsUnique();
            builder.HasIndex(p => p.Email).IsUnique();

            builder.Property(p => p.RegistrationDate).HasDefaultValueSql("GETDATE()");



        }
    }
}
