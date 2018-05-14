using PG.Model;

namespace PG.Repository
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        UserProfile GetByUserName(string userName);
    }
}
