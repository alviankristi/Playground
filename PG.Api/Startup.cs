using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PG.Api.Identity;
using PG.DataAccess;
using System;

[assembly: OwinStartup(typeof(PG.Api.Startup))]

namespace PG.Api
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static string PublicClientId { get; } = "self";

        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration httpConfig = new HttpConfiguration();
            //WebApiConfig.Register(httpConfig);

            app.CreatePerOwinContext(PlaygroundDbContext.Create);
            app.CreatePerOwinContext<UserManager>(UserManager.Create);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(OAuthOptions);

            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}