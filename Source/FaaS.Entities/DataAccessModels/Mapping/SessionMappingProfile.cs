using AutoMapper;

namespace FaaS.Entities.DataAccessModels.Mapping
{
    public class SessionMappingProfile : Profile
    {
        public SessionMappingProfile()
        {
            CreateMap<Session, DataTransferModels.Session>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled));

            CreateMap<DataTransferModels.Session, Session>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Filled, opt => opt.MapFrom(src => src.Filled))
                .ForMember(dst => dst.ElementValues, opt => opt.Ignore());
        }
    }
}
