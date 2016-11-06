using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;
using FaaS.Entities.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;

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
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="formRepository">form repository</param>
        /// <param name="projectRepository">project repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public FormService(IFormRepository formRepository, IProjectRepository projectRepository, ILogger<IFormService> logger, IMapper mapper)
        {
            this.formRepository = formRepository;
            this.projectRepository = projectRepository;
            this.logger = logger;
            this.mapper = mapper;
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

            var dataAccessProjectModel = mapper.Map<Entities.DataAccessModels.Project>(project);
            var dataAccessFormModel = mapper.Map<Entities.DataAccessModels.Form>(form);
            dataAccessFormModel = await formRepository.Add(dataAccessProjectModel, dataAccessFormModel);

            return mapper.Map<Form>(dataAccessFormModel);
        }

        public async Task<Form> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            var dataAccessFormModel = await formRepository.Get(id);

            return mapper.Map<Form>(dataAccessFormModel);
        }

        public async Task<Form[]> GetAll()
        {
            logger.LogInformation("GetAll operation was called");

            var dataAccessFormModel = await formRepository.List();

            return mapper.Map<Form[]>(dataAccessFormModel);
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

            //Project dataAccessProjectModel = _mapper.Map<Project>(project);
            var dataAccessFormModel = await formRepository.List(/*dataAccessProjectModel*/existingProject);

            return mapper.Map<Form[]>(dataAccessFormModel);
        }

        public async Task<Form> Remove(Form form)
        {
            logger.LogInformation("Remove operation was called");

            var dataAccessFormModel = mapper.Map<Entities.DataAccessModels.Form>(form);
            dataAccessFormModel = await formRepository.Delete(dataAccessFormModel);

            return mapper.Map<Form>(dataAccessFormModel);
        }

        public async Task<Form> Update(Form form)
        {
            logger.LogInformation("Update operation was called");

            var dataAccessFormModel = mapper.Map<Entities.DataAccessModels.Form>(form);
            dataAccessFormModel = await formRepository.Update(dataAccessFormModel);

            return mapper.Map<Form>(dataAccessFormModel);
        }
    }
}
