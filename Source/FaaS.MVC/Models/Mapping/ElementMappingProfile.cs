using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ElementMappingProfile : Profile
    {
        public ElementMappingProfile()
        {
            CreateMap<DataTransferModels.Element, ElementViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
               .ForMember(dst => dst.Required, opt => opt.MapFrom(src => src.Required))
               .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<ElementViewModel, DataTransferModels.Element>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))

               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
               .ForMember(dst => dst.Required, opt => opt.MapFrom(src => src.Required))
               .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dst => dst.Form, opt => opt.Ignore());

            CreateMap<DataTransferModels.Element, ElementDetailsViewModel>()
                .IncludeBase<DataTransferModels.Element, ElementViewModel>()
                .ForMember(dst => dst.FormId, opt => opt.MapFrom(src => src.Form.Id));

            CreateMap<CreateElementViewModel, DataTransferModels.Element>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Required, opt => opt.MapFrom(src => src.Required))
                .ForMember(dst => dst.Form, opt => opt.Ignore())
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
