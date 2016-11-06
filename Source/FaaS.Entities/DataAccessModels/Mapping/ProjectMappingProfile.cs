using AutoMapper;

namespace FaaS.Entities.DataAccessModels.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, DataTransferModels.Project>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.ProjectName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User));

            CreateMap<DataTransferModels.Project, Project>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.ProjectName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.Forms, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.Ignore());
        }
    }
}
