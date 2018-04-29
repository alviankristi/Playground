using PG.DataAccess;
using PG.Model;
using PG.Repository.Cache;

namespace PG.Repository
{
    public class SiteRepository : BaseRepository<Site>, ISiteRepository
    {
        public SiteRepository(IPlaygroundDbContext dbContext)
            : base(dbContext)
        {

        }

        public SiteRepository(IPlaygroundDbContext dbContext, ICacheService cacheService)
            : base(dbContext, cacheService)
        {

        }
        
    }
}
