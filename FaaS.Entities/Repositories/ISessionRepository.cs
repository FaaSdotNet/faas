using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllSessions();
       
        Task<Session> AddSession(Session session);

        Task<Session> DeleteSession(Session session);
    }
}
