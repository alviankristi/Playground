using PG.Model;
using PG.Repository;

namespace PG.BLL
{
    public class UserProfileService : BaseService<UserProfile, IUserProfileRepository>, IUserProfileService
    {
        public UserProfileService(IUserProfileRepository repository) : base(repository)
        {
        }

        public override UserProfile GetById(int id)
        {
            return Repo.Get(id, u => u.AppUser);
        }

        public UserProfile GetByUserName(string userName)
        {
            return Repo.GetByUserName(userName);
        }
    }
}
