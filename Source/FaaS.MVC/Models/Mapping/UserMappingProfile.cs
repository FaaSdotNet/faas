using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataTransferModels.User, UserViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<UserViewModel, DataTransferModels.User>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<DataTransferModels.User, UserDetailsViewModel>()
                .IncludeBase<DataTransferModels.User, UserViewModel>();

            CreateMap<CreateUserViewModel, DataTransferModels.User>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));
        }
    }
}
