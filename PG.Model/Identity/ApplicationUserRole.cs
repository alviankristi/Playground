using Microsoft.AspNet.Identity.EntityFramework;

namespace PG.Model
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        
    }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        
    }
}
