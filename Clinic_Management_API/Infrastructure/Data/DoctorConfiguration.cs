using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FullName).HasMaxLength(50);
            builder.Property(d => d.Phone).HasMaxLength(30);
            builder.Property(d => d.Email).HasMaxLength(320);

            builder.HasIndex(d => d.Phone).IsUnique();
            builder.HasIndex(d => d.Email).IsUnique();
            builder.HasIndex(d => d.LicenseNumber).IsUnique();

            builder.Property(d => d.ConsultationFee)
                .HasPrecision(10, 2);

            builder.HasOne(d => d.Specialty)
                .WithMany(d => d.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
