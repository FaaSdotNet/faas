using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
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
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectRepository">project repository</param>
        /// <param name="userRepository">user repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, ILogger<IProjectService> logger, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Project> Add(User user, Project project)
        {
            logger.LogInformation("Add operation called");

            var existingUser = await userRepository.Get(user.GoogleId);
            if (existingUser == null)
            {
                var message = $"User with Google ID = [{user.GoogleId}] does not exist.";
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

            var dataAccessUserModel = mapper.Map<Entities.DataAccessModels.User>(user);
            var dataAccessProjectModel = mapper.Map<Entities.DataAccessModels.Project>(project);
            dataAccessProjectModel = await projectRepository.Add(dataAccessUserModel, dataAccessProjectModel);

            return mapper.Map<Project>(dataAccessProjectModel);
        }


        public async Task<Project> Get(Guid id)
        {
            logger.LogInformation("Get operation called");

            var dataAccessProjectModel = await projectRepository.Get(id);
            return mapper.Map<Project>(dataAccessProjectModel);
        }

        public async Task<Project[]> GetAllForUser(User user)
        {
            logger.LogInformation("GetAll operation was called");

            var existingUser = await userRepository.Get(user.GoogleId);
            if (existingUser == null)
            {
                var message = $"User with Google ID = [{user.GoogleId}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataAccessUserModel = mapper.Map<Entities.DataAccessModels.User>(user);
            var dataAccessProjectModel = await projectRepository.List(dataAccessUserModel);

            return mapper.Map<Project[]>(dataAccessProjectModel);
        }

        public async Task<Project> Remove(Project project)
        {
            logger.LogInformation("Remove was called");

            var dataAccessProjectModel = mapper.Map<Entities.DataAccessModels.Project>(project);
            dataAccessProjectModel = await projectRepository.Delete(dataAccessProjectModel);

            return mapper.Map<Project>(dataAccessProjectModel);
        }

        public async Task<Project> Update(Project project)
        {
            logger.LogInformation("Update was called");

            var dataAccessProjectModel = mapper.Map<Entities.DataAccessModels.Project>(project);
            dataAccessProjectModel = await projectRepository.Update(dataAccessProjectModel);

            return mapper.Map<Project>(dataAccessProjectModel);
        }
    }
}
