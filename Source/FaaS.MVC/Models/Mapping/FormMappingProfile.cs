using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class FormMappingProfile : Profile
    {
        public FormMappingProfile()
        {
            CreateMap<DataTransferModels.Form, FormViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.FormName, opt => opt.MapFrom(src => src.FormName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<FormViewModel, DataTransferModels.Form>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.FormName, opt => opt.MapFrom(src => src.FormName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.Project, opt => opt.Ignore());

            CreateMap<DataTransferModels.Form, FormDetailsViewModel>()
                .IncludeBase<DataTransferModels.Form, FormViewModel>()
                .ForMember(dst => dst.ProjectId, opt => opt.MapFrom(src => src.Project.Id));

            CreateMap<CreateFormViewModel, DataTransferModels.Form>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.FormName, opt => opt.MapFrom(src => src.FormName))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Project, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}