using AutoMapper;
using PG.Api.DtoModels;
using PG.Model;

namespace PG.Api
{
    public class SiteMappingProfile : Profile
    {
        public SiteMappingProfile()
        {
            CreateMap<NewSiteDto, Site>();
            CreateMap<EditSiteDto, Site>();
            CreateMap<SiteDto, Site>().ReverseMap();
        }
    }
}