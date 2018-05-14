using Microsoft.AspNet.Identity.EntityFramework;

namespace PG.Model
{
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public bool IsActive { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
