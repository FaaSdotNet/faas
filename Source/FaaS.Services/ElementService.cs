using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;
using AutoMapper;

namespace FaaS.Services
{
    /// <summary>
    /// Implementation of <see cref="IElementService"/> interface
    /// </summary>
    public class ElementService : IElementService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IElementService> logger;

        /// <summary>
        /// Element repository
        /// </summary>
        private readonly IElementRepository elementRepository;

        /// <summary>
        /// Form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elementRepository">element repository</param>
        /// <param name="formRepository">form repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public ElementService(IElementRepository elementRepository, IFormRepository formRepository, ILogger<IElementService> logger, IMapper mapper)
        {
            this.elementRepository = elementRepository;
            this.formRepository = formRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Element> Add(Form form, Element element)
        {
            logger.LogInformation("Add operation called");

            var existingForm = await formRepository.Get(form.Id);
            if (existingForm == null)
            {
                var message = $"Form with ID = [{form.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingElement = await elementRepository.Get(element.Id);
            if (existingElement != null)
            {
                var message = $"Element with ID = [{element.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataAccessFormModel = mapper.Map<Entities.DataAccessModels.Form>(existingForm);
            var dataAccessElementModel = mapper.Map<Entities.DataAccessModels.Element>(element);
            dataAccessElementModel = await elementRepository.Add(dataAccessFormModel, dataAccessElementModel);

            return mapper.Map<Element>(dataAccessElementModel);
        }

        public async Task<Element> Get(Guid id)
        {
            logger.LogInformation("Get operation called");

            var dataAccessElementModel = await elementRepository.Get(id);

            return mapper.Map<Element>(dataAccessElementModel);
        }

        public async Task<Element[]> GetAll()
        {
            logger.LogInformation("GetAll operation called");

            var dataAccessElementModel = await elementRepository.GetAll();

            return mapper.Map<Element[]>(dataAccessElementModel);
        }

        public async Task<Element[]> GetAllForForm(Form form)
        {
            logger.LogInformation("GetAllForForm operation called");

            var existingForm = await formRepository.Get(form.Id);
            if (existingForm == null)
            {
                var message = $"Form with ID = [{form.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataAccessFormModel = mapper.Map<Entities.DataAccessModels.Form>(existingForm);
            var dataAccessElementModel = await elementRepository.GetAll(dataAccessFormModel);

            return mapper.Map<Element[]>(dataAccessElementModel);
        }

        public async Task<Element> Remove(Element element)
        {
            logger.LogInformation("Remove operation called");

            var dataAccessElementModel = mapper.Map<Entities.DataAccessModels.Element>(element);
            dataAccessElementModel = await elementRepository.Delete(dataAccessElementModel);

            return mapper.Map<Element>(dataAccessElementModel);
        }

        public async Task<Element> Update(Element element)
        {
            logger.LogInformation("Update operation called");

            var dataAccessElementModel = mapper.Map<Entities.DataAccessModels.Element>(element);
            dataAccessElementModel = await elementRepository.Update(dataAccessElementModel);

            return mapper.Map<Element>(dataAccessElementModel);
        }
    }
}
