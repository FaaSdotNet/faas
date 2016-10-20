using AutoMapper;

namespace FaaS.Services.DataTransferModels.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Entities.DataAccessModels.Project, Project>()
                .ForMember(dst => dst.ProjectCodeName, opt => opt.MapFrom(src => src.CodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User));

            CreateMap<Project, Entities.DataAccessModels.Project>()
                .ForMember(dst => dst.CodeName, opt => opt.MapFrom(src => src.ProjectCodeName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dst => dst.Forms, opt => opt.Ignore())
                .ForMember(dst => dst.Id, opt => opt.Ignore());
        }
    }
}
