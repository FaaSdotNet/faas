using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public Task<Project> AddProject(User user, Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Project> DeleteProject(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllProjects()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllProjects(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetSingleProject(string name)
        {
            throw new NotImplementedException();
        }
    }
}
