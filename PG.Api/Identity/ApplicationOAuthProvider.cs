using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PG.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PG.Api.Identity
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException(nameof(publicClientId));
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userManager = context.OwinContext.GetUserManager<UserManager>())
            {
                ApplicationUser user = await userManager.FindByNameAsync(context.UserName);
                bool passwordValid = false;

                if (user != null)
                {
                    passwordValid = await userManager.CheckPasswordAsync(user, context.Password);
                }

                if (user == null || !passwordValid)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                //Add this to check if the email was confirmed.
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "Email address has not been confirmed.");
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError("invalid_grant", "The user is inactive, please contact administrator.");
                    return;
                }

                ClaimsIdentity oAuthIdentity = await userManager.GenerateUserIdentityAsync(user, context.Options.AuthenticationType);

                ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);

                List<Claim> roles = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                string permissions = oAuthIdentity.Claims.FirstOrDefault(c => c.Type == "Permissions")?.Value ?? string.Empty;

                AuthenticationProperties properties = CreateProperties(user.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value)), permissions);

                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };

            return new AuthenticationProperties(data);
        }

        public static AuthenticationProperties CreateProperties(string userName, string roles, string permissions)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"userName", userName },
                {"roles", roles},
                {"permissions", permissions},
            };

            return new AuthenticationProperties(data);
        }
    }
}