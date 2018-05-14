using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using PG.DataAccess;
using PG.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PG.Api.Identity
{
    public class UserManager : UserManager<ApplicationUser, int>
    {
        public UserManager(IUserStore<ApplicationUser, int> store) : base(store)
        {
        }

        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<PlaygroundDbContext>();
            var appUserManager = new UserManager(new UserStore(appDbContext));

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return appUserManager;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser user, string authenticationType)
        {
            var userIdentity = await CreateIdentityAsync(user, authenticationType);

            //userIdentity.AddClaim(new Claim(CustomClaimType.Permissions, GetPermissionClaimValue()));
            return userIdentity;
        }
    }
}
