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
    /// Implementation of <see cref="ISessionService"/> interface
    /// </summary>
    public class SessionService : ISessionService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ISessionService> logger;

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
        /// <param name="sessionRepository">session repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public SessionService(ISessionRepository sessionRepository, ILogger<ISessionService> logger, IMapper mapper)
        {
            this.sessionRepository = sessionRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<Session> Add(Session session)
        {
            logger.LogInformation("Add operation was called");

            var existingSession = await sessionRepository.Get(session.Id);
            if (existingSession != null)
            {
                var message = $"Session with ID = [{session.Id}] already exists.";
                logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataAccessSessionModel = mapper.Map<Entities.DataAccessModels.Session>(session);
            dataAccessSessionModel = await sessionRepository.Add(dataAccessSessionModel);

            return mapper.Map<Session>(dataAccessSessionModel);
        }

        public async Task<Session> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            var dataAccessSessionModel = await sessionRepository.Get(id);

            return mapper.Map<Session>(dataAccessSessionModel);
        }

        public async Task<Session[]> GetAll()
        {
            logger.LogInformation("GetAll operation was called");

            var dataAccessSessionModel = await sessionRepository.List();
            return mapper.Map<Session[]>(dataAccessSessionModel);
        }

        public async Task<Session> Remove(Session session)
        {
            logger.LogInformation("Remove operation was called");

            var dataAccessSessionModel = mapper.Map<Entities.DataAccessModels.Session>(session);
            dataAccessSessionModel = await sessionRepository.Delete(dataAccessSessionModel);

            return mapper.Map<Session>(dataAccessSessionModel);
        }

        public async Task<Session> Update(Session session)
        {
            logger.LogInformation("Update operation was called");

            var dataAccessSessionModel = mapper.Map<Entities.DataAccessModels.Session>(session);
            dataAccessSessionModel = await sessionRepository.Update(dataAccessSessionModel);

            return mapper.Map<Session>(dataAccessSessionModel);
        }
    }
}
