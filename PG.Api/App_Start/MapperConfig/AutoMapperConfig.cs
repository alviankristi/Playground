using AutoMapper;

namespace PG.Api
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<FacilityMappingProfile>();
                config.AddProfile<SiteMappingProfile>();
                config.AddProfile<UserProfileMappingProfile>();
            });
        }
    }
}