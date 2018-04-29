using Autofac;
using Autofac.Integration.WebApi;
using PG.BLL;
using PG.DataAccess;
using PG.Repository;
using System.Reflection;
using System.Web.Http;
using PG.Repository.Cache;

namespace PG.Api
{
    public class AutofacConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterInstance(new PlaygroundDbContext()).As<IPlaygroundDbContext>();

            if (ApplicationSetting.EnableCache)
                builder.RegisterInstance(new RedisCacheService(ApplicationSetting.CacheConnection)).As<ICacheService>();

            builder.RegisterType<FacilityRepository>().As<IFacilityRepository>().InstancePerRequest();
            builder.RegisterType<SiteRepository>().As<ISiteRepository>().InstancePerRequest();

            builder.RegisterType<FacilityService>().As<IFacilityService>().InstancePerRequest();
            builder.RegisterType<SiteService>().As<ISiteService>().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Container = builder.Build();

            return Container;
        }
    }
}