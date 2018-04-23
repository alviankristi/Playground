using PG.Model;

namespace PG.Api.DtoModels
{
    public class NewSiteDto : BaseNewDto<Site>
    {
        public string Name { get; set; }
    }

    public class EditSiteDto : BaseDto<Site>
    {
        public string Name { get; set; }
    }

    public class SiteDto : EditSiteDto
    {
        public override void LoadFromEntity(Site entity)
        {
            base.LoadFromEntity(entity);

            Name = entity.Name;
        }
    }
}