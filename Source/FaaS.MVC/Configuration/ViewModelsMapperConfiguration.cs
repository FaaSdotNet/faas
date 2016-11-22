using AutoMapper;
using FaaS.MVC.Models.Mapping;
using FaaS.Services;

namespace FaaS.MVC.Configuration
{
    public class ViewModelsMapperConfiguration
    {
        public static void InitializeMappings(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<ProjectMappingProfile>();
            cfg.AddProfile<FormMappingProfile>();
            cfg.AddProfile<ElementMappingProfile>();
            cfg.AddProfile<ElementValueMappingProfile>();
            cfg.AddProfile<SessionMappingProfile>();
        }
    }
}
