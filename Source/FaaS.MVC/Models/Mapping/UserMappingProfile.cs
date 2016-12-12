using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataTransferModels.User, UserViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dst => dst.GoogleToken, opt => opt.MapFrom(src => src.GoogleToken))
               .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dst => dst.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));

            CreateMap<UserViewModel, DataTransferModels.User>()
               .ForMember(dst => dst.Id, opt => opt.Ignore())
               .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dst => dst.GoogleToken, opt => opt.MapFrom(src => src.GoogleToken))
               .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dst => dst.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
               .ForMember(dst => dst.Registered, opt => opt.MapFrom(src => src.Registered));
        }
    }
}
