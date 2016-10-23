using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ElementValueMappingProfile : Profile
    {
        public ElementValueMappingProfile()
        {
            CreateMap<Services.DataTransferModels.ElementValue, ElementValueViewModel>()
               .ForMember(dst => dst.ElementValueCodeName, opt => opt.MapFrom(src => src.ElementValueCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.ElementName, opt => opt.MapFrom(src => src.Element.ElementCodeName))
               .ForMember(dst => dst.SessionName, opt => opt.MapFrom(src => src.Session.SessionCodeName));
        }
    }
}
