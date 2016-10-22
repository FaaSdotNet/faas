using AutoMapper;
using FaaS.MVC.Models.Projects;

namespace FaaS.MVC.Models.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Services.DataTransferModels.Project, ProjectModelView>()
               .ForMember(dst => dst.ProjectCodeName, opt => opt.MapFrom(src => src.ProjectCodeName))
               .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User.DisplayName))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
