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
    public class SchoolInfoConfiguration : IEntityTypeConfiguration<SchoolInfo>
    {
        public void Configure(EntityTypeBuilder<SchoolInfo> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.NumberOfPeople)
                   .IsRequired();

            builder.Property(s => s.Year)
                   .IsRequired();

            builder.HasOne(s => s.Vehicles)
                   .WithOne()
                   .HasForeignKey<SchoolInfo>(s => s.CampusVehicleEntryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("SchoolInfos");
        }
    }

}
