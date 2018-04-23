using AutoMapper;
using PG.Model;

namespace PG.Api.DtoModels
{
    public abstract class BaseNewDto<TEntity>
        where TEntity : BaseModel, new()
    {
        public virtual TEntity ToEntity()
        {
            return Mapper.Map<TEntity>(this);
        }
    }

    public abstract class BaseDto<TEntity> : BaseNewDto<TEntity>
        where TEntity : BaseModel, new()
    {
        public int Id { get; set; }

        public virtual TEntity ToEntity(TEntity originalEntity)
        {
            var updatedEntity = originalEntity;

            Mapper.Map(this, updatedEntity);

            return updatedEntity;
        }

        public virtual void LoadFromEntity(TEntity entity)
        {
            Id = entity.Id;
        }
    }
}