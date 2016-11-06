using System;
using FaaS.DataTransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> Get(Guid id);
    
        Task<IEnumerable<Project>> List();

        Task<IEnumerable<Project>> List(User user);

        Task<Project> Add(User user, Project project);

        Task<Project> Update(Project project);

        Task<Project> Delete(Project project);
     }
}
