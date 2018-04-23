using AutoMapper;
using PG.Model;
using System.Data.Spatial;

namespace PG.Api.DtoModels
{
    public class NewFacilityDto : BaseNewDto<Facility>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public DbGeography Location { get; set; }
    }

    public class EditFacilityDto : BaseDto<Facility>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        public DbGeography Location { get; set; }
    }

    public class FacilityDto : EditFacilityDto
    {
        public SiteDto Site { get; set; }
        
        public override void LoadFromEntity(Facility entity)
        {
            base.LoadFromEntity(entity);

            Name = entity.Name;
            Description = entity.Description;
            Images = entity.Images;

            if (entity.Site != null)
                Site = Mapper.Map<Site, SiteDto>(entity.Site);

            if (entity.Location != null)
                Location = DbGeography.FromText(entity.Location.AsText());
        }
    }
}