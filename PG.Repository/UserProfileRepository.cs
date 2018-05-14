using Newtonsoft.Json;
using PG.DataAccess;
using PG.Model;
using PG.Repository.Cache;
using System.Data.Entity;
using System.Linq;

namespace PG.Repository
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IPlaygroundDbContext dbContext) : base(dbContext)
        {
        }

        public UserProfileRepository(IPlaygroundDbContext dbContext, ICacheService cacheService) : base(dbContext, cacheService)
        {
        }
        
        public UserProfile GetByUserName(string userName)
        {
            UserProfile entity;

            var cachedEntity = Cache?.Get($"{SingleCacheKeyPrefix}:{userName}");
            if (string.IsNullOrEmpty(cachedEntity))
            {
                entity = Db.Set<UserProfile>().Include(e => e.AppUser)
                    .FirstOrDefault(e => e.AppUser.UserName == userName);

                Cache?.Add($"{SingleCacheKeyPrefix}:{userName}", JsonConvert.SerializeObject(entity));
            }
            else
            {
                entity = JsonConvert.DeserializeObject<UserProfile>(cachedEntity);
            }

            return entity;
        }
    }
}
