using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Entities.DataAccessModels;
using FaaS.MVC.Models.Mapping;

namespace FaaS.MVC.Configuration
{
    public class ViewModelsMapperConfiguration
    {
        public static void InitialializeMappings(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<ProjectMappingProfile>();
        }
    }
}
