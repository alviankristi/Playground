using PG.DataAccess.ModelConfigs;
using PG.Model;
using System.Data.Entity;

namespace PG.DataAccess
{
    public class PlaygroundDbContext : DbContext, IPlaygroundDbContext
    {
        public PlaygroundDbContext() : base("DefaultConnection")
        {
            
        }

        public PlaygroundDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            
        }

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Site> Sites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FacilityConfig());
            modelBuilder.Configurations.Add(new SiteConfig());
        }
    }
}
