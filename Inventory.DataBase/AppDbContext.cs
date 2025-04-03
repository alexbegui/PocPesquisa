using CensusFieldSurvey.Model.EntitesBD;
using Microsoft.EntityFrameworkCore;

namespace CensusFieldSurvey.DataBase
{
    public class AppDbContext : DbContext
    {
        public DbSet<Research> Researchs { get; set; }

        public AppDbContext()
        {
            Database.Migrate();
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Host=localhost;Port=5432;Database=API2;Username=postgres;Password=postgres";

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}