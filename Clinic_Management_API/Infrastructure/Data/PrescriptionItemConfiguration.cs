using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class PrescriptionItemConfiguration : IEntityTypeConfiguration<PrescriptionItem>
    {
        public void Configure(EntityTypeBuilder<PrescriptionItem> builder)
        {
            builder.HasKey(pi => new { pi.PrescriptionId, pi.MedicationId });

            builder.Property(p => p.Dosage).HasMaxLength(50);
            builder.Property(p => p.Frequency).HasMaxLength(50);
            builder.Property(p => p.Duration).HasMaxLength(50);
            builder.Property(p => p.Instructions).HasMaxLength(120);

            builder.HasOne(p => p.Prescription)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(p => p.PrescriptionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Medication)
                .WithMany(p => p.PrescriptionItems)
                .HasForeignKey(p => p.MedicationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
