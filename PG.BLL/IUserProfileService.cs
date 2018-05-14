using PG.Model;

namespace PG.BLL
{
    public interface IUserProfileService : IService<UserProfile>
    {
        UserProfile GetByUserName(string userName);
    }
}
