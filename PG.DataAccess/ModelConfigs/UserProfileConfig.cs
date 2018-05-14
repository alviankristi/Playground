using PG.Model;
using System.Data.Entity.ModelConfiguration;

namespace PG.DataAccess.ModelConfigs
{
    public class UserProfileConfig : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfig()
        {
            HasRequired(profile => profile.AppUser)
                .WithOptional(user => user.UserProfile)
                .Map(profile => profile.MapKey("AppUserId"))
                .WillCascadeOnDelete();
        }
    }
}
