using System;
using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> Get(Guid userId, string name);
        Task<Project> Get(User user, string name);
        Task<Project> Get(Guid id);
    
        Task<IEnumerable<Project>> List();

        Task<IEnumerable<Project>> List(User user);

        Task<Project> Add(User user, Project project);
        Task<Project> Update(Project project);

        Task<Project> Delete(Project project);
     }
}
