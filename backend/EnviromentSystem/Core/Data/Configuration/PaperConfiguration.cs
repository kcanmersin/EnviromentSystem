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
    public class PaperConfiguration : IEntityTypeConfiguration<Paper>
    {
        public void Configure(EntityTypeBuilder<Paper> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Date)
                   .IsRequired();

            builder.Property(p => p.Usage)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.ToTable("Papers");
        }
    }
}
