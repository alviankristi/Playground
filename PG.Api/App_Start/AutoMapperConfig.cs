using System.Data.Entity.Spatial;
using AutoMapper;
using PG.Api.DtoModels;
using PG.Model;

namespace PG.Api
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<NewSiteDto, Site>();
                config.CreateMap<EditSiteDto, Site>();
                config.CreateMap<SiteDto, Site>().ReverseMap();

                config.CreateMap<NewFacilityDto, Facility>()
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => DbGeography.FromText(src.Location.AsText())));
                config.CreateMap<EditFacilityDto, Facility>()
                    .ForMember(dest => dest.Location,
                        opt => opt.MapFrom(src => DbGeography.FromText(src.Location.AsText())));
                config.CreateMap<FacilityDto, Facility>().ReverseMap();
            });
        }
    }
}