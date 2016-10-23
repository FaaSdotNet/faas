using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ElementMappingProfile : Profile
    {
        public ElementMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Element, ElementViewModel>()
               .ForMember(dst => dst.ElementCodeName, opt => opt.MapFrom(src => src.ElementCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
               .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Mandatory))
               .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<Services.DataTransferModels.Element, ElementDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.Element, ElementViewModel>()
                .ForMember(dst => dst.FormName, opt => opt.MapFrom(src => src.Form.FormCodeName));

            CreateMap<CreateElementViewModel, Services.DataTransferModels.Element>()
                .ForMember(dst => dst.ElementCodeName, opt => opt.MapFrom(src => src.ElementCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Mandatory))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
