using System;
using System.Linq;
using System.Threading.Tasks;
using FaaS.DataTransferModels;
using FaaS.Entities.Repositories;
using Microsoft.Extensions.Logging;

namespace FaaS.Services
{
    public class FormService : IFormService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IFormService> logger;

        /// <summary>
        /// Form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// Project repository
        /// </summary>
        private readonly IProjectRepository projectRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="formRepository">form repository</param>
        /// <param name="projectRepository">project repository</param>
        /// <param name="logger">logger</param>
        public FormService(IFormRepository formRepository, IProjectRepository projectRepository, ILogger<IFormService> logger)
        {
            this.formRepository = formRepository;
            this.projectRepository = projectRepository;
            this.logger = logger;
        }

        public async Task<Form> Add(Project project, Form form)
        {
            logger.LogInformation("Add operation was called");

            var existingProject = await projectRepository.Get(project.Id);
            if (existingProject == null)
            {
                var message = $"Project with ID = [{project.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingForm = await formRepository.Get(form.Id);
            if (existingForm != null)
            {
                var message = $"Form with ID = [{form.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return await formRepository.Add(existingProject, form);
        }

        public async Task<Form> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            return await formRepository.Get(id);
        }

        public async Task<Form[]> GetAll()
        {
            logger.LogInformation("GetAll operation was called");

            return (await formRepository.List()).ToArray();
        }

        public async Task<Form[]> GetAllForProject(Project project)
        {
            logger.LogInformation("GetAllForProject operation was called");

            var existingProject = await projectRepository.Get(project.Id);
            if (existingProject == null)
            {
                var message = $"Project with ID = [{project.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return (await formRepository.List(existingProject)).ToArray();
        }

        public async Task<Form> Remove(Form form)
        {
            logger.LogInformation("Remove operation was called");

            return await formRepository.Delete(form);
        }

        public async Task<Form> Update(Form form)
        {
            logger.LogInformation("Update operation was called");

            return await formRepository.Update(form);
        }
    }
}
