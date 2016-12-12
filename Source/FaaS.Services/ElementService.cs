using System;
using System.Linq;
using System.Threading.Tasks;
using FaaS.DataTransferModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;

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
        /// Constructor
        /// </summary>
        /// <param name="elementRepository">element repository</param>
        /// <param name="formRepository">form repository</param>
        /// <param name="logger">logger</param>
        public ElementService(IElementRepository elementRepository, IFormRepository formRepository, ILogger<IElementService> logger)
        {
            this.elementRepository = elementRepository;
            this.formRepository = formRepository;
            this.logger = logger;
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
            /*
            var existingElement = await elementRepository.Get(element.Id);
            if (existingElement != null)
            {
                var message = $"Element with ID = [{element.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }*/

            return await elementRepository.Add(existingForm, element);
        }

        public async Task<Element> Get(Guid id)
        {
            logger.LogInformation("Get operation called");

            return await elementRepository.Get(id);
        }

        public async Task<Element[]> GetAll()
        {
            logger.LogInformation("GetAll operation called");

            return (await elementRepository.GetAll()).ToArray();
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

            return (await elementRepository.GetAll(existingForm)).ToArray();
        }

        public async Task<Element> Remove(Element element)
        {
            logger.LogInformation("Remove operation called");

            return await elementRepository.Delete(element);
        }

        public async Task<Element> Update(Element element)
        {
            logger.LogInformation("Update operation called");

            return await elementRepository.Update(element);
        }
    }
}
