using PG.Api.DtoModels;
using PG.BLL;
using PG.Common;
using PG.Model;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace PG.Api.Controllers
{
    [RoutePrefix("Site")]
    public class SiteController : BaseController<NewSiteDto, EditSiteDto, SiteDto, Site, ISiteService>
    {
        public SiteController(ISiteService siteService)
            : base(siteService)
        {
            
        }

        [Route("{id}", Name = "GetSiteById")]
        public override IHttpActionResult Get(int id)
        {
            return base.Get(id);
        }

        [Route("")]
        public override IHttpActionResult Post([FromBody] NewSiteDto value)
        {
            return base.Post(value);
        }

        [Route("{id}")]
        public override IHttpActionResult Put(int id, [FromBody] EditSiteDto value)
        {
            return base.Put(id, value);
        }

        [Route("{id}")]
        public override IHttpActionResult Delete(int id)
        {
            return base.Delete(id);
        }

        [Route("n/{name}", Name = "GetSiteByName")]
        public IHttpActionResult Get(string name)
        {
            var list = Svc.GetByName(name);

            var source = list.Items.Select(i =>
            {
                var item = new SiteDto();
                item.LoadFromEntity(i);

                return item;
            });

            return Json(new PagedList<SiteDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [Route("{id}/Facilities"), HttpGet]
        public IHttpActionResult GetFacilities(int id)
        {
            var list = Svc.GetFacilities(id);

            var source = list.Items.Select(i =>
            {
                var item = new FacilityDto();
                item.LoadFromEntity(i);

                return item;
            });

            return Json(new PagedList<FacilityDto>(source, list.PageIndex, list.PageSize, list.TotalCount));
        }

        [Route("{id}/AddFacility"), HttpPost]
        public IHttpActionResult AddFacility(int id, [FromBody] NewFacilityDto value)
        {
            var entity = Svc.GetById(id);
            if (entity == null)
                return NotFound();

            var facility = value.ToEntity();
            int facilityId = Svc.AddFacility(id, facility);

            var createdDto = new FacilityDto();
            createdDto.LoadFromEntity(facility);

            return CreatedAtRoute("GetFacilityById", new {id = facilityId}, createdDto);
        }

        [Route("{id}/RemoveFacility/{facilityId}"), HttpDelete]
        public IHttpActionResult RemoveFacility(int id, int facilityId)
        {
            Svc.RemoveFacility(id, facilityId);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}