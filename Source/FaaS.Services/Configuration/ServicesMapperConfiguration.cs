using AutoMapper;
using FaaS.Services.DataTransferModels.Mapping;

namespace FaaS.Services.Configuration
{
    public static class ServicesMapperConfiguration
    {
        public static void InitializeMappings(IMapperConfigurationExpression configuration)
        {
            configuration.AddProfile<UserMappingProfile>();
            configuration.AddProfile<ProjectMappingProfile>();
            configuration.AddProfile<FormMappingProfile>();
            configuration.AddProfile<ElementMappingProfile>();
            configuration.AddProfile<ElementValueMappingProfile>();
            configuration.AddProfile<SessionMappingProfile>();
        }
    }
}
