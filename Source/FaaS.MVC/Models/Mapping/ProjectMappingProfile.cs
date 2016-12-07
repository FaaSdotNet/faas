using AutoMapper;

namespace FaaS.MVC.Models.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<DataTransferModels.Project, ProjectViewModel>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.ProjectName, opt => opt.MapFrom(src => src.ProjectName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<ProjectViewModel, DataTransferModels.Project>()
               .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dst => dst.ProjectName, opt => opt.MapFrom(src => src.ProjectName))
               .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
               .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dst => dst.User, opt => opt.Ignore());
        }
    }
}
