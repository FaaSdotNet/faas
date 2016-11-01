using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.Session, Session>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled));

            CreateMap<Session, Entities.DataAccessModels.Session>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled))
                .ForMember(dst => dst.ElementValues, opt => opt.Ignore());
        }
    }
}
