using AutoMapper;
using PG.Api.DtoModels;
using PG.Model;
using System.Data.Entity.Spatial;

namespace PG.Api
{
    public class FacilityMappingProfile : Profile
    {
        public FacilityMappingProfile()
        {
            CreateMap<NewFacilityDto, Facility>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => DbGeography.FromText(src.Location.AsText())));
            CreateMap<EditFacilityDto, Facility>()
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(src => DbGeography.FromText(src.Location.AsText())));
            CreateMap<FacilityDto, Facility>().ReverseMap();
        }
    }
}