using Autofac;
using PG.BLL;

namespace PG.Api
{
    public static class ServiceConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<FacilityService>().As<IFacilityService>().InstancePerRequest();
            builder.RegisterType<SiteService>().As<ISiteService>().InstancePerRequest();
            builder.RegisterType<UserProfileService>().As<IUserProfileService>().InstancePerRequest();
        }
    }
}