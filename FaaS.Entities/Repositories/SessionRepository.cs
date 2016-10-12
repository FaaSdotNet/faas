using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public Task<Session> AddSession(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<Session> DeleteSession(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Session>> GetAllSessions()
        {
            throw new NotImplementedException();
        }
    }
}
