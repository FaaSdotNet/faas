using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Services.DataTransferModels.User, UserViewModel>()
               .ForMember(dst => dst.UserCodeName, opt => opt.MapFrom(src => src.UserCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<Services.DataTransferModels.User, UserDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.User, UserViewModel>();

            CreateMap<CreateUserViewModel, Services.DataTransferModels.User>()
                .ForMember(dst => dst.UserCodeName, opt => opt.MapFrom(src => src.UserCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));
        }
    }
}
