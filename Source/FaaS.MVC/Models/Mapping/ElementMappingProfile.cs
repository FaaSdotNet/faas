using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ElementMappingProfile : Profile
    {
        public ElementMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Element, ElementViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
               .ForMember(dst => dst.Required, opt => opt.MapFrom(src => src.Mandatory))
               .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<ElementViewModel, Services.DataTransferModels.Element>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))

               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
               .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Required))
               .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dst => dst.Form, opt => opt.Ignore());

            CreateMap<Services.DataTransferModels.Element, ElementDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.Element, ElementViewModel>()
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.Form.Id));

            CreateMap<CreateElementViewModel, Services.DataTransferModels.Element>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Mandatory, opt => opt.MapFrom(src => src.Required))
                .ForMember(dst => dst.Form, opt => opt.Ignore())
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
