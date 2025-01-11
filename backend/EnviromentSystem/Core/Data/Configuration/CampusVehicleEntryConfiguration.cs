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
    public class CampusVehicleEntryConfiguration : IEntityTypeConfiguration<CampusVehicleEntry>
    {
        public void Configure(EntityTypeBuilder<CampusVehicleEntry> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CarsManagedByUniversity)
                   .IsRequired();

            builder.Property(c => c.CarsEnteringUniversity)
                   .IsRequired();

            builder.Property(c => c.MotorcyclesEnteringUniversity)
                   .IsRequired();

            builder.ToTable("CampusVehicleEntries");
        }
    }

}
