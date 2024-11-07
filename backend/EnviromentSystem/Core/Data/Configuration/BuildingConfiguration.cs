using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Data.Entity;

namespace Core.Data.Configuration
{
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.E_MeterCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(b => b.Electrics)
                   .WithOne(e => e.Building)
                   .HasForeignKey(e => e.BuildingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Buildings");
        }
    }
}
