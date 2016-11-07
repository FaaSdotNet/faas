using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<DataTransferModels.Session, SessionViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.ElementValueList, opt => opt.Ignore())
               .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled));
        }
    }
}
