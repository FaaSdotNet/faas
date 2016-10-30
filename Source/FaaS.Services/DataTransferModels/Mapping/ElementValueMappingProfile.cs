﻿using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class ElementValueMappingProfile : Profile
    {
        public ElementValueMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.ElementValue, ElementValue>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dst => dst.Element, opt => opt.MapFrom(src => src.Element))
                .ForMember(dst => dst.Session, opt => opt.MapFrom(src => src.Session));

            CreateMap<ElementValue, Entities.DataAccessModels.ElementValue>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dst => dst.Element, opt => opt.MapFrom(src => src.Element))
                .ForMember(dst => dst.Session, opt => opt.MapFrom(src => src.Session))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.ElementId, opt => opt.Ignore())
                .ForMember(dst => dst.SessionId, opt => opt.Ignore());
        }
    }
}
