using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class ElementMappingProfile : Profile
    {
        public ElementMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.Element, Element>()
                .ForMember(dst => dst.ElementCodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Mandatory))
                .ForMember(dst => dst.Form, opt => opt.MapFrom(src => src.Form));

            CreateMap<Element, Entities.DataAccessModels.Element>()
                .ForMember(dst => dst.CodeName, opt => opt.MapFrom(src => src.ElementCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Mandatory))
                .ForMember(dst => dst.Form, opt => opt.MapFrom(src => src.Form))
                .ForMember(dst => dst.ElementValues, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
