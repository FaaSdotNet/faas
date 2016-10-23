using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Project, ProjectViewModel>()
               .ForMember(dst => dst.ProjectCodeName, opt => opt.MapFrom(src => src.ProjectCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Services.DataTransferModels.Project, ProjectDetailsViewModel>()
                .IncludeBase<Services.DataTransferModels.Project, ProjectViewModel>()
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.User.UserCodeName));

            CreateMap<CreateProjectViewModel, Services.DataTransferModels.Project>()
                .ForMember(dst => dst.ProjectCodeName, opt => opt.MapFrom(src => src.ProjectCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.User, opt => opt.Ignore())
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
