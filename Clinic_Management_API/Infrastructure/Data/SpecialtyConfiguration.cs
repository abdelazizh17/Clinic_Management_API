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

            builder.HasData(
               new Specialty { Id = 1, Name = "Cardiology", Description = "Heart and cardiovascular system care" },
               new Specialty { Id = 2, Name = "Dermatology", Description = "Skin, hair, and nail treatments" },
               new Specialty { Id = 3, Name = "Pediatrics", Description = "Medical care for infants, children, and adolescents" },
               new Specialty { Id = 4, Name = "Neurology", Description = "Brain and nervous system disorders" },
               new Specialty { Id = 5, Name = "Orthopedics", Description = "Bones, joints, and musculoskeletal system care" }
            );
        }
    }
}
