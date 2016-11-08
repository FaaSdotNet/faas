using System;
using System.Threading.Tasks;

using FaaS.DataTransferModels;

namespace FaaS.Services.Interfaces
{
    public interface ISessionService
    {
        /// <summary>
        /// Adds a new session to the database
        /// </summary>
        /// <param name="session">new session</param>
        /// <returns>Newly added session</returns>
        Task<Session> Add(Session session);

        /// <summary>
        /// Gets a session with given id
        /// </summary>
        /// <param name="id">id of the session</param>
        /// <returns>session with given id</returns>
        Task<Session> Get(Guid id);

        /// <summary>
        /// Gets all sessions
        /// </summary>
        /// <returns>all sessions</returns>
        Task<Session[]> GetAll();

        /// <summary>
        /// Updates a session
        /// </summary>
        /// <param name="session">session to be updated</param>
        /// <returns>updated session</returns>
        Task<Session> Update(Session session);

        /// <summary>
        /// Removes a session
        /// </summary>
        /// <param name="session">session to be removed</param>
        /// <returns>removed session</returns>
        Task<Session> Remove(Session session);
    }
}
