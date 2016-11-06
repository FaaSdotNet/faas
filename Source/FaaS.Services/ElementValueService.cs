using FaaS.Services.Interfaces;
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
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elementValueRepository">element value repository</param>
        /// <param name="elementRepository">element repository</param>
        /// <param name="sessionRepository">session repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public ElementValueService(IElementValueRepository elementValueRepository, IElementRepository elementRepository, ISessionRepository sessionRepository, ILogger<IElementValueService> logger, IMapper mapper)
        {
            this.elementValueRepository = elementValueRepository;
            this.elementRepository = elementRepository;
            this.sessionRepository = sessionRepository;
            this.logger = logger;
            this.mapper = mapper;
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

            var dataAccessElementModel = mapper.Map<Entities.DataAccessModels.Element>(element);
            var dataAccessSessionModel = mapper.Map<Entities.DataAccessModels.Session>(session);
            var dataAccessElementValueModel = mapper.Map<Entities.DataAccessModels.ElementValue>(elementValue);
            dataAccessElementValueModel = await elementValueRepository.Add(dataAccessElementModel, dataAccessSessionModel, dataAccessElementValueModel);
        
            return mapper.Map<ElementValue>(dataAccessElementValueModel);
        }

        public async Task<ElementValue> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            var dataAccessElementValueModel = await elementValueRepository.Get(id);

            return mapper.Map<ElementValue>(dataAccessElementValueModel);
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

            var dataAccessElementModel = mapper.Map<Entities.DataAccessModels.Element>(element);
            var dataAccessElementValueModel = await elementValueRepository.List(dataAccessElementModel);

            return mapper.Map<ElementValue[]>(dataAccessElementValueModel);
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

            var dataAccessSessionModel = mapper.Map<Entities.DataAccessModels.Session>(session);
            var dataAccessElementValueModel = await elementValueRepository.List(dataAccessSessionModel);

            return mapper.Map<ElementValue[]>(dataAccessElementValueModel);
        }

        public async Task<ElementValue> Remove(ElementValue elementValue)
        {
            logger.LogInformation("Remove operation was called");

            var dataAccessElementValueModel = mapper.Map<Entities.DataAccessModels.ElementValue>(elementValue);
            dataAccessElementValueModel = await elementValueRepository.Delete(dataAccessElementValueModel);

            return mapper.Map<ElementValue>(dataAccessElementValueModel);
        }

        public async Task<ElementValue> Update(ElementValue elementValue)
        {
            logger.LogInformation("Update operation was called");

            var dataAccessElementValueModel = mapper.Map<Entities.DataAccessModels.ElementValue>(elementValue);
            dataAccessElementValueModel = await elementValueRepository.Update(dataAccessElementValueModel);

            return mapper.Map<ElementValue>(dataAccessElementValueModel);
        }
    }
}
