using PG.Api.DtoModels;
using PG.BLL;
using PG.Common;
using PG.Model;
using System.Net;
using System.Web.Http;

namespace PG.Api.Controllers
{
    public abstract class BaseController<TNewDto, TEditDto, TDto, TEntity, TService> : ApiController
        where TEntity : BaseModel, new()
        where TNewDto : BaseNewDto<TEntity> 
        where TEditDto : BaseDto<TEntity> 
        where TDto : BaseDto<TEntity>, new()
        where TService : IService<TEntity>
    {
        protected TService Svc;
        protected ILogger Logger;

        protected BaseController(TService service)
        {
            Svc = service;
            Logger = LoggerManager.GetLogger();
        }

        protected BaseController(TService service, ILogger logger)
        {
            Svc = service;
            Logger = logger;
        }
        
        public virtual IHttpActionResult Get(int id)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var item = new TDto();
            item.LoadFromEntity(entity);
            
            return Ok(item);
        }
        
        public virtual IHttpActionResult Post([FromBody]TNewDto value)
        {
            var newEntity = value.ToEntity();
            var id = Svc.Create(newEntity);

            var item = new TDto();
            item.LoadFromEntity(newEntity);
            
            return CreatedAtRoute("GetById", new {id}, item);
        }
        
        public virtual IHttpActionResult Put(int id, [FromBody]TEditDto value)
        {
            if (id != value.Id)
                return BadRequest();

            var originalEntity = Svc.GetById(id);
            var updatedEntity = value.ToEntity(originalEntity);

            Svc.Update(updatedEntity);

            var item = new TDto();
            item.LoadFromEntity(updatedEntity);

            return Ok(item);
        }
        
        public virtual IHttpActionResult Delete(int id)
        {
            Svc.Delete(id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}