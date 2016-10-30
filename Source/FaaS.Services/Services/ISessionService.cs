using System;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;

namespace FaaS.Services
{
    public interface ISessionService
    {
        Task<Session> Add(Session session);
        Task<Session> Get(Guid id);
        Task<Session[]> GetAll();
        Task<Session> Update(Session session);
        Task<Session> Remove(Session session);
    }
}