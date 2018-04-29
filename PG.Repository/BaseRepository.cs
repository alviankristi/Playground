using PG.Common;
using PG.Common.Extensions;
using PG.DataAccess;
using PG.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using PG.Repository.Cache;

namespace PG.Repository
{
    public abstract class BaseRepository<TEntity> where TEntity: BaseModel
    {
        protected IPlaygroundDbContext Db;
        protected ICacheService Cache;

        protected virtual string SingleCacheKeyPrefix => typeof(TEntity).FullName?.Replace(".", ":");

        protected BaseRepository(IPlaygroundDbContext dbContext)
        {
            Db = dbContext;
        }

        protected BaseRepository(IPlaygroundDbContext dbContext, ICacheService cacheService)
        {
            Db = dbContext;
            Cache = cacheService;
        }

        public virtual int Create(TEntity newEntity)
        {
            var dbEntity = GetEntity(newEntity);
            dbEntity.State = EntityState.Added;
            
            Db.SaveChanges();

            var id = newEntity.Id;

            Cache?.Add($"{SingleCacheKeyPrefix}:{id}", JsonConvert.SerializeObject(newEntity));

            return id;
        }

        public virtual void Delete(int id)
        {
            TEntity entity = Get(id);
            if (entity != null)
            {
                Db.Entry(entity).State = EntityState.Deleted;
                Db.SaveChanges();

                Cache?.Delete($"{SingleCacheKeyPrefix}:{id}");
            }
        }

        public virtual PagedList<TEntity> Filter<TKey>(int pageIndex, int pageSize, OrderBySelector<TEntity, TKey> orderBySelector, Expression<Func<TEntity, bool>> whereFilter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = Db.Set<TEntity>();
            foreach (var prop in includeProperties)
            {
                entities = entities.Include(prop);
            }

            entities = orderBySelector.Type == OrderByType.Ascending
                ? entities.OrderBy(orderBySelector.Selector)
                : entities.OrderByDescending(orderBySelector.Selector);

            var query = whereFilter != null ? entities.Where(whereFilter) : entities;

            return query.ToPagedList(pageIndex, pageSize);
        }

        public virtual TEntity Get(int id)
        {
            TEntity entity;

            var cachedEntity = Cache?.Get($"{SingleCacheKeyPrefix}:{id}");
            if (string.IsNullOrEmpty(cachedEntity))
            {
                entity = Db.Set<TEntity>().Find(id);

                Cache?.Add($"{SingleCacheKeyPrefix}:{id}", JsonConvert.SerializeObject(entity));
            }
            else
            {
                entity = JsonConvert.DeserializeObject<TEntity>(cachedEntity);
            }
            
            return entity;
        }

        public virtual TEntity Get(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var pagedList = Filter(1, 1, 
                new OrderBySelector<TEntity, int>(OrderByType.Ascending, entity => entity.Id),
                entity => entity.Id == id, includeProperties);

            return pagedList.TotalCount > 0 ? pagedList.Items.FirstOrDefault() : null;
        }

        public virtual void Update(TEntity updatedEntity)
        {
            var dbEntity = GetEntity(updatedEntity);
            dbEntity.State = EntityState.Modified;

            Db.SaveChanges();

            Cache?.Delete($"{SingleCacheKeyPrefix}:{updatedEntity.Id}");
        }

        private DbEntityEntry<TEntity> GetEntity(TEntity entity)
        {
            var dbEntity = Db.Entry(entity);
            if (dbEntity.State == EntityState.Detached)
                Db.Set<TEntity>().Attach(entity);
            return dbEntity;
        }
    }
}
