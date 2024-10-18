using Microsoft.EntityFrameworkCore;
using Core.Data.Entity;

namespace Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<SchoolInfo> SchoolInfos { get; set; }
        public DbSet<Electric> Electrics { get; set; }
        public DbSet<Water> Waters { get; set; }
        public DbSet<Paper> Papers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the relationships and constraints
            modelBuilder.Entity<SchoolInfo>()
                .HasMany(s => s.Electrics)
                .WithOne(e => e.SchoolInfo)
                .HasForeignKey(e => e.SchoolInfoId);

            modelBuilder.Entity<SchoolInfo>()
                .HasMany(s => s.Waters)
                .WithOne(w => w.SchoolInfo)
                .HasForeignKey(w => w.SchoolInfoId);

            modelBuilder.Entity<SchoolInfo>()
                .HasMany(s => s.Papers)
                .WithOne(p => p.SchoolInfo)
                .HasForeignKey(p => p.SchoolInfoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
