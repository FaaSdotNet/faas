using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Session, SessionViewModel>()
               .ForMember(dst => dst.SessionCodeName, opt => opt.MapFrom(src => src.SessionCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled));
        }
    }
}
