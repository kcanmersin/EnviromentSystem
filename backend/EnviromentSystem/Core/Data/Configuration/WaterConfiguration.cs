using Core.Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Configuration
{
    public class WaterConfiguration : IEntityTypeConfiguration<Water>
    {
        public void Configure(EntityTypeBuilder<Water> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Date)
                   .IsRequired();

            builder.Property(w => w.InitialMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(w => w.FinalMeterValue)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(w => w.Usage)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.ToTable("Waters");
        }
    }
}
