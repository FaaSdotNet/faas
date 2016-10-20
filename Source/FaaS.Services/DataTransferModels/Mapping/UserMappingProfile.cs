using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.User, User>()
                .ForMember(dst => dst.UserCodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<User, Entities.DataAccessModels.User>()
                .ForMember(dst => dst.CodeName, opt => opt.MapFrom(src => src.UserCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered))
                .ForMember(dst => dst.Projects, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
