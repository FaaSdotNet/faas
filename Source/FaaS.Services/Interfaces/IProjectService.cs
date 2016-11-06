using FaaS.Services.DataTransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Adds a new project
        /// </summary>
        /// <param name="user">user that owns the project</param>
        /// <param name="project">project</param>
        /// <returns>Newly created project</returns>
        Task<Project> Add(User user, Project project);
        
        /// <summary>
        /// Gets a project
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns>Project with given id</returns>
        Task<Project> Get(Guid id);

        /// <summary>
        /// Gets all project belonging to user given
        /// </summary>
        /// <param name="user">user that owns the projects</param>
        /// <returns>All projects belonging to given user</returns>
        Task<Project[]> GetAllForUser(User user);

        /// <summary>
        /// Updates the existing project
        /// </summary>
        /// <param name="project">project to be updated</param>
        /// <returns>Updated project</returns>
        Task<Project> Update(Project project);

        /// <summary>
        /// Removes the project
        /// </summary>
        /// <param name="project">project to be removed</param>
        /// <returns>Removed project</returns>
        Task<Project> Remove(Project project);
    }
}
