using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.User, User>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<User, Entities.DataAccessModels.User>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered))
                .ForMember(dst => dst.Projects, opt => opt.Ignore());
        }
    }
}
