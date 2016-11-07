using FaaS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.DataTransferModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;

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
        /// Constructor
        /// </summary>
        /// <param name="sessionRepository">session repository</param>
        /// <param name="logger">logger</param>
        public SessionService(ISessionRepository sessionRepository, ILogger<ISessionService> logger)
        {
            this.sessionRepository = sessionRepository;
            this.logger = logger;
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

            return await sessionRepository.Add(session);
        }

        public async Task<Session> Get(Guid id)
        {
            logger.LogInformation("Get operation was called");

            return await sessionRepository.Get(id);
        }

        public async Task<Session[]> GetAll()
        {
            logger.LogInformation("GetAll operation was called");

            return (await sessionRepository.List()).ToArray();
        }

        public async Task<Session> Remove(Session session)
        {
            logger.LogInformation("Remove operation was called");

            return await sessionRepository.Delete(session);
        }

        public async Task<Session> Update(Session session)
        {
            logger.LogInformation("Update operation was called");

            return await sessionRepository.Update(session);
        }
    }
}
