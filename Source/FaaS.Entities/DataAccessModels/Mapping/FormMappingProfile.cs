using AutoMapper;

namespace FaaS.Entities.DataAccessModels.Mapping
{
    public class FormMappingProfile : Profile
    {
        public FormMappingProfile()
        {
            CreateMap<Form, DataTransferModels.Form>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.FormName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Project, opt => opt.MapFrom(src => src.Project));

            CreateMap<DataTransferModels.Form, Form>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.FormName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Elements, opt => opt.Ignore())
                .ForMember(dst => dst.ProjectId, opt => opt.Ignore());
        }
    }
}
