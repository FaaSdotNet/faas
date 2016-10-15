using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> GetSingleProject(string name);

        Task<IEnumerable<Project>> GetAllProjects();

        Task<IEnumerable<Project>> GetAllProjects(User user);

        Task<Project> AddProject(User user, Project project);

        Task<Project> DeleteProject(Project project);
     }
}
