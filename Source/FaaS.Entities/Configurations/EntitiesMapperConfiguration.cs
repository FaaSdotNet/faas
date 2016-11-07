using AutoMapper;
using FaaS.Entities.DataAccessModels.Mapping;

namespace FaaS.Entities.Configuration
{
    public static class EntitiesMapperConfiguration
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
