using PG.Model;
using System.Data.Entity.ModelConfiguration;

namespace PG.DataAccess.ModelConfigs
{
    public class FacilityConfig : EntityTypeConfiguration<Facility>
    {
        public FacilityConfig()
        {
            HasRequired(facility => facility.Site)
                .WithMany(site => site.Facilities)
                .HasForeignKey(facility => facility.SiteId);
        }
    }
}
