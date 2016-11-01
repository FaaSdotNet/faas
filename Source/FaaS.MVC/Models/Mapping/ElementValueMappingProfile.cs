using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ElementValueMappingProfile : Profile
    {
        public ElementValueMappingProfile()
        {
            CreateMap<Services.DataTransferModels.ElementValue, ElementValueViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.ElementId, opt => opt.MapFrom(src => src.Element.Id))
               .ForMember(dst => dst.SessionId, opt => opt.MapFrom(src => src.Session.Id));
        }
    }
}
