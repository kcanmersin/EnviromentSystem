using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Core.Data.Configuration;

namespace Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Electric> Electrics { get; set; }
        public DbSet<Water> Waters { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<SchoolInfo> SchoolInfos { get; set; }
        public DbSet<NaturalGas> NaturalGasUsages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new ElectricConfiguration());
            modelBuilder.ApplyConfiguration(new NaturalGasConfiguration());

            modelBuilder.Entity<Water>().ToTable("Waters");
            modelBuilder.Entity<Paper>().ToTable("Papers");
            modelBuilder.Entity<SchoolInfo>().ToTable("SchoolInfos");
            modelBuilder.Entity<NaturalGas>().ToTable("NaturalGasUsages"); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
