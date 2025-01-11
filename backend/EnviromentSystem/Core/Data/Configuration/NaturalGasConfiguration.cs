using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class NaturalGasConfiguration : IEntityTypeConfiguration<NaturalGas>
    {
        public void Configure(EntityTypeBuilder<NaturalGas> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Date)
                   .IsRequired();

            builder.Property(n => n.InitialMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(n => n.FinalMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(n => n.Usage)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(n => n.SM3Value)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(n => n.Building)
                   .WithMany(b => b.NaturalGasUsages) 
                   .HasForeignKey(n => n.BuildingId)
                   .OnDelete(DeleteBehavior.Cascade);

  
            builder.ToTable("NaturalGas"); 
        }
    }
}
