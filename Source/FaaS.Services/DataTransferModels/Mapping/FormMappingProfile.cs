using AutoMapper;
using System;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class FormMappingProfile : Profile
    {
        public FormMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.Form, Form>()
                .ForMember(dst => dst.FormCodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Project, opt => opt.MapFrom(src => src.Project));

            CreateMap<Form, Entities.DataAccessModels.Form>()
                .ForMember(dst => dst.CodeName, opt => opt.MapFrom(src => src.FormCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Elements, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
