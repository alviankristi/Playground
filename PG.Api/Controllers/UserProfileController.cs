using PG.Api.DtoModels;
using PG.BLL;
using PG.Model;
using System.Web.Http;

namespace PG.Api.Controllers
{
    [RoutePrefix("UserProfile")]
    public class UserProfileController : BaseController<UserProfileDto, EditUserProfileDto, UserProfileDto, UserProfile, IUserProfileService>
    {
        public UserProfileController(IUserProfileService service) : base(service)
        {
        }
        
        [Authorize]
        [Route("{id}", Name = "GetUserProfileById")]
        public override IHttpActionResult Get(int id)
        {
            return base.Get(id);
        }

        [Authorize]
        [Route("{id}")]
        public override IHttpActionResult Put(int id, [FromBody] EditUserProfileDto value)
        {
            return base.Put(id, value);
        }

        [Authorize]
        [Route("u/{username}", Name = "GetUserProfileByUserName")]
        public IHttpActionResult Get(string username)
        {
            var entity = Svc.GetByUserName(username);
            if (entity == null)
                return NotFound();

            var item = new UserProfileDto();
            item.LoadFromEntity(entity);
            
            return Ok(item);
        }
    }
}