using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;

namespace FaaS.Services
{
    public interface IProjectService
    {
        Task<Project> Add(User user, Project project);

        Task<Project> Get(User u, string name);
        Task<Project> Get(Guid id);

        Task<Project[]> GetAll(User user);

        Task<Project> Update(Project project);

        Task<Project> Remove(Project project);
    }
}
