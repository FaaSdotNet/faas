using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<Services.DataTransferModels.User, UserViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<Services.DataTransferModels.User, UserDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.User, UserViewModel>();

            CreateMap<CreateUserViewModel, Services.DataTransferModels.User>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dst => dst.GoogleId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));
        }
    }
}
