using Microsoft.AspNet.Identity.EntityFramework;
using PG.DataAccess;
using PG.Model;

namespace PG.Api.Identity
{
    public class UserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public UserStore(PlaygroundDbContext context) : base(context)
        {
        }
    }
}