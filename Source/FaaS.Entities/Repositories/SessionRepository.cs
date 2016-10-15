using FaaS.Entities.Configuration;
using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private FaaSContext _context;

        public SessionRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Session> AddSession(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            Session addedSession = _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return addedSession;
        }

        public async Task<Session> DeleteSession(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            Session deletedSession = _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return deletedSession;
        }

        public async Task<IEnumerable<Session>> GetAllSessions()
            => await _context.Sessions.ToArrayAsync();
    }
}
