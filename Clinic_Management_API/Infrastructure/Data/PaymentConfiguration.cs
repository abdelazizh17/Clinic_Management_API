using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Infrastructure.Data
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Appointment)
              .WithOne(p => p.Payment)
              .HasForeignKey<Payment>(p => p.AppointmentId)
              .IsRequired()
              .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            builder.Property(p => p.Amount).HasPrecision(10, 2);

            builder.Property(p => p.PaymentDate).HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
