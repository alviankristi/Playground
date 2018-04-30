using Autofac;
using PG.Repository;
using PG.Repository.Cache;

namespace PG.Api
{
    public static class RepositoryConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            if (ApplicationSetting.EnableCache)
                builder.RegisterInstance(new RedisCacheService(ApplicationSetting.CacheConnection)).As<ICacheService>();

            builder.RegisterType<FacilityRepository>().As<IFacilityRepository>().InstancePerRequest();
            builder.RegisterType<SiteRepository>().As<ISiteRepository>().InstancePerRequest();
        }
    }
}