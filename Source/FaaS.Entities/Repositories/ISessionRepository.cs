using System;
using FaaS.DataTransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> List();
       
        Task<Session> Add(Session session);

        Task<Session> Delete(Session session);

        Task<Session> Update(Session session);

        Task<Session> Get(Guid id);
    }
}
