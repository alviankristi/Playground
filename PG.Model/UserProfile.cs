namespace PG.Model
{
    public class UserProfile : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ApplicationUser AppUser { get; set; }
    }
}
