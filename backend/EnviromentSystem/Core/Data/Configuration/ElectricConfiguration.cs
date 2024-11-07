using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Data.Entity;

namespace Core.Data.Configuration
{
    public class ElectricConfiguration : IEntityTypeConfiguration<Electric>
    {
        public void Configure(EntityTypeBuilder<Electric> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Date)
                   .IsRequired();

            builder.Property(e => e.InitialMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.FinalMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Usage)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.KWHValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(e => e.Building)
                   .WithMany(b => b.Electrics)
                   .HasForeignKey(e => e.BuildingId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.ToTable("Electrics");
        }
    }
}
