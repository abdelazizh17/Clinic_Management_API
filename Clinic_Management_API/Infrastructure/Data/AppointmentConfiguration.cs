using Clinic_Management_API.Core.Entities;


namespace Clinic_Management_API.Infrastructure.Data
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentDate).HasColumnType("date");
            builder.Property(a => a.StartTime).HasColumnType("time");
            builder.Property(a => a.EndTime).HasColumnType("time");

            builder.Property(a => a.Notes).HasMaxLength(500);

            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(a => a.Patient)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.PatientId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            builder.HasOne(a => a.Doctor)
                .WithMany(a => a.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

          
        }
    }
}
