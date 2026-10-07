using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Notes).HasMaxLength(500);

            builder.Property(p => p.PrescriptionDate).HasColumnType("date");

            builder.HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Doctor)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.DoctorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(p => p.Appointment)
                .WithOne(a => a.Prescription)
                .HasForeignKey<Prescription>(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
