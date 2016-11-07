using AutoMapper;

namespace FaaS.Entities.DataAccessModels.Mapping
{
    public class ElementValueMappingProfile : Profile
    {
        public ElementValueMappingProfile()
        {
            CreateMap<ElementValue, DataTransferModels.ElementValue>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dst => dst.Element, opt => opt.MapFrom(src => src.Element))
                .ForMember(dst => dst.Session, opt => opt.MapFrom(src => src.Session));

            CreateMap<DataTransferModels.ElementValue, ElementValue>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dst => dst.Element, opt => opt.MapFrom(src => src.Element))
                .ForMember(dst => dst.Session, opt => opt.MapFrom(src => src.Session))
                .ForMember(dst => dst.ElementId, opt => opt.Ignore())
                .ForMember(dst => dst.SessionId, opt => opt.Ignore());
        }
    }
}
