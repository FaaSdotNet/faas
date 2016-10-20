using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.Session, Session>()
                .ForMember(dst => dst.SessionCodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled));

            CreateMap<Session, Entities.DataAccessModels.Session>()
                .ForMember(dst => dst.CodeName, opt => opt.MapFrom(src => src.SessionCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled))
                .ForMember(dst => dst.ElementValues, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
