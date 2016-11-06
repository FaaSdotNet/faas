using AutoMapper;
using FaaS.Entities.Configuration;
using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using FaaS.Entities.DataAccessModels.Mapping;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly FaaSContext _context;
        private IMapper _mapper;

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal SessionRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
            var config = new MapperConfiguration(cfg => EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public SessionRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
        }

        public async Task<DataTransferModels.Session> Add(DataTransferModels.Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            Session dataAccessSessionModel = _mapper.Map<Session>(session);

            var addedSession = _context.Sessions.Add(dataAccessSessionModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Session>(addedSession);
        }

        public async Task<DataTransferModels.Session> Delete(DataTransferModels.Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            Session oldSession = _context.Sessions.SingleOrDefault(sessionToDelete => sessionToDelete.Id == session.Id);
            if (oldSession == null)
            {
                throw new ArgumentException("Session not in db!");
            }

            Session deletedSession = _context.Sessions.Remove(oldSession);

            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Session>(deletedSession);
        }

        public async Task<IEnumerable<DataTransferModels.Session>> List()
        {
            var sessions = await _context.Sessions.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Session>>(sessions);
        }

        public async Task<DataTransferModels.Session> Update(DataTransferModels.Session updatedSession)
        {
            if (updatedSession == null)
            {
                throw new ArgumentNullException(nameof(updatedSession));
            }

            Session oldSession = _context.Sessions.SingleOrDefault(session => session.Id == updatedSession.Id);
            if (oldSession == null)
            {
                throw new ArgumentException("Session is not in db!");
            }

            oldSession.Filled = updatedSession.Filled;
            _context.Entry(oldSession).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Session>(oldSession);
        }

        public async Task<DataTransferModels.Session> Get(Guid id)
        {
            Session session = await _context.Sessions.SingleOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<DataTransferModels.Session>(session);
        }
    }
}
