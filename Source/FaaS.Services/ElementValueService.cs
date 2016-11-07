using FaaS.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using FaaS.DataTransferModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;

namespace FaaS.Services
{
    /// <summary>
    /// Implementation of <see cref="IElementValueService"/> interface
    /// </summary>
    public class ElementValueService : IElementValueService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IElementValueService> logger;

        /// <summary>
        /// Element value repository
        /// </summary>
        private readonly IElementValueRepository elementValueRepository;

        /// <summary>
        /// Element repository
        /// </summary>
        private readonly IElementRepository elementRepository;

        /// <summary>
        /// Session repository
        /// </summary>
        private readonly ISessionRepository sessionRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elementValueRepository">element value repository</param>
        /// <param name="elementRepository">element repository</param>
        /// <param name="sessionRepository">session repository</param>
        /// <param name="logger">logger</param>
        public ElementValueService(IElementValueRepository elementValueRepository, IElementRepository elementRepository, ISessionRepository sessionRepository, ILogger<IElementValueService> logger)
        {
            this.elementValueRepository = elementValueRepository;
            this.elementRepository = elementRepository;
            this.sessionRepository = sessionRepository;
            this.logger = logger;
        }

        public async Task<ElementValue> Add(Element element, Session session, ElementValue elementValue)
        {
            logger.LogInformation("Add operation was called");

            var existingElement = await elementRepository.Get(element.Id);
            if (existingElement == null)
            {
                var message = $"Element with ID = [{element.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingSession = await sessionRepository.Get(session.Id);
            if (existingSession == null)
            {
                var message = $"Session with ID = [{session.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingElementValue = await elementValueRepository.Get(elementValue.Id);
            if (existingElementValue != null)
            {
                var message = $"Element value with ID = [{elementValue.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return await elementValueRepository.Add(existingElement, existingSession, elementValue);
        }

        public async Task<ElementValue> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            return await elementValueRepository.Get(id);
        }

        public async Task<ElementValue[]> GetAllForElement(Element element)
        {
            logger.LogInformation("GetAllForElement operation was called");

            var existingElement = await elementRepository.Get(element.Id);
            if (existingElement == null)
            {
                var message = $"Element with ID = [{element.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return (await elementValueRepository.List(element)).ToArray();
        }

        public async Task<ElementValue[]> GetAllForSession(Session session)
        {
            logger.LogInformation("GetAllForSession operation was called");

            var existingSession = await sessionRepository.Get(session.Id);
            if (existingSession == null)
            {
                var message = $"Session with ID = [{session.Id}] does not exist.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return (await elementValueRepository.List(existingSession)).ToArray();
        }

        public async Task<ElementValue> Remove(ElementValue elementValue)
        {
            logger.LogInformation("Remove operation was called");

            return await elementValueRepository.Delete(elementValue);
        }

        public async Task<ElementValue> Update(ElementValue elementValue)
        {
            logger.LogInformation("Update operation was called");
            
            return await elementValueRepository.Update(elementValue);
        }
    }
}
