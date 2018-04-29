using PG.DataAccess;
using PG.Model;
using PG.Repository.Cache;

namespace PG.Repository
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
    {
        public FacilityRepository(IPlaygroundDbContext dbContext) : base(dbContext)
        {
            
        }

        public FacilityRepository(IPlaygroundDbContext dbContext, ICacheService cacheService) : base(dbContext, cacheService)
        {
        }
    }
}
