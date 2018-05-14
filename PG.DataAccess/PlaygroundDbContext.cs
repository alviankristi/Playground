using Microsoft.AspNet.Identity.EntityFramework;
using PG.DataAccess.ModelConfigs;
using PG.Model;
using System.Data.Entity;

namespace PG.DataAccess
{
    public class PlaygroundDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IPlaygroundDbContext
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
        public DbSet<UserProfile> UserProfiles { get; set; }

        public static PlaygroundDbContext Create()
        {
            return new PlaygroundDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new FacilityConfig());
            modelBuilder.Configurations.Add(new SiteConfig());
            modelBuilder.Configurations.Add(new UserProfileConfig());
        }
    }
}
