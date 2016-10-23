using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class FormMappingProfile : Profile
    {
        public FormMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Form, FormViewModel>()
               .ForMember(dst => dst.FormCodeName, opt => opt.MapFrom(src => src.FormCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Services.DataTransferModels.Form, FormDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.Form, FormViewModel>()
                .ForMember(dst => dst.ProjectName, opt => opt.MapFrom(src => src.Project.ProjectCodeName));

            CreateMap<CreateFormViewModel, Services.DataTransferModels.Form>()
                .ForMember(dst => dst.FormCodeName, opt => opt.MapFrom(src => src.FormCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}