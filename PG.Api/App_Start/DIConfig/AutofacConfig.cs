using Autofac;
using Autofac.Integration.WebApi;
using PG.Common;
using PG.DataAccess;
using System.Reflection;
using System.Web.Http;

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

            LoggerManager.AddLogger(LoggerManager.DefaultLoggerName, container.Resolve<ILogger>());
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterInstance(new PlaygroundDbContext()).As<IPlaygroundDbContext>();
            builder.RegisterType<NLogLogger>().As<ILogger>().SingleInstance();
            
            RepositoryConfig.Register(builder);

            ServiceConfig.Register(builder);

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Container = builder.Build();

            return Container;
        }
    }
}