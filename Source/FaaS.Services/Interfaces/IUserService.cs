using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FaaS.DataTransferModels;

namespace FaaS.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">new user</param>
        /// <returns>Newly created user</returns>
        Task<User> Add(User user);

        /// <summary>
        /// Gets user with given globally unique id
        /// </summary>
        /// <param name="id">guid of the user</param>
        /// <returns>user with given id</returns>
        Task<User> Get(Guid id);

        /// <summary>
        /// Gets user with given google id
        /// </summary>
        /// <param name="googleId">google id of the user</param>
        /// <returns>user with given google id</returns>
        Task<User> Get(string googleId);

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>All users</returns>
        Task<User[]> GetAll();

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">user to be updated</param>
        /// <returns>Updated user</returns>
        Task<User> Update(User user);

        /// <summary>
        /// Removes the user
        /// </summary>
        /// <param name="user">user to be removed</param>
        /// <returns>Removed user</returns>
        Task<User> Remove(User user);
    }
}
