using PG.Api.DtoModels;
using PG.BLL;
using PG.Model;
using System.Web.Http;

namespace PG.Api.Controllers
{
    [RoutePrefix("Facility")]
    public class FacilityController : BaseController<NewFacilityDto, EditFacilityDto, FacilityDto, Facility, IFacilityService>
    {
        public FacilityController(IFacilityService service) : base(service)
        {
        }

        [Route("{id}", Name = "GetFacilityById")]
        public override IHttpActionResult Get(int id)
        {
            return base.Get(id);
        }

        [Route("")]
        public override IHttpActionResult Post([FromBody] NewFacilityDto value)
        {
            return base.Post(value);
        }

        [Route("{id}")]
        public override IHttpActionResult Put(int id, [FromBody] EditFacilityDto value)
        {
            return base.Put(id, value);
        }

        [Route("{id}")]
        public override IHttpActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}