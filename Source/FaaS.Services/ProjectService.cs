using System;
using System.Linq;
using System.Threading.Tasks;
using FaaS.DataTransferModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;

namespace FaaS.Services
{
    /// <summary>
    /// Implementation of <see cref="IProjectService"/> interface
    /// </summary>
    public class ProjectService : IProjectService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IProjectService> logger;

        /// <summary>
        /// Project repository
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectRepository">project repository</param>
        /// <param name="userRepository">user repository</param>
        /// <param name="logger">logger</param>
        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, ILogger<IProjectService> logger)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Project> Add(User user, Project project)
        {
            logger.LogInformation("Add operation called");

            var existingUser = await userRepository.Get(user.Id);
            if (existingUser == null)
            {
                var message = $"User with Google ID = [{user.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingProject = await projectRepository.Get(project.Id);
            if (existingProject != null)
            {
                var message = $"Project with ID = [{project.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return await projectRepository.Add(existingUser, project);
        }


        public async Task<Project> Get(Guid id)
        {
            logger.LogInformation("Get operation called");

            return await projectRepository.Get(id);
        }

        public async Task<Project[]> GetAllForUser(User user)
        {
            logger.LogInformation("GetAll operation was called");

            var existingUser = await userRepository.Get(user.Id);
            if (existingUser == null)
            {
                var message = $"User with ID = [{user.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }
           
            return (await projectRepository.List(existingUser)).ToArray();
        }

        public async Task<Project> Remove(Project project)
        {
            logger.LogInformation("Remove was called");
           
            return await projectRepository.Delete(project);
        }

        public async Task<Project> Update(Project project)
        {
            logger.LogInformation("Update was called");

            return await projectRepository.Update(project);
        }
    }
}
