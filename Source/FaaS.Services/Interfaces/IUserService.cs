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
        /// Adds a new user to the database, if user exists, it updates the google token
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
        /// Gets user with given email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>user with given email</returns>
        Task<User> Get(string email);

        /// <summary>
        /// Gets user with given token
        /// </summary>
        /// <param name="token">user token</param>
        /// <returns>user with given token</returns>
        Task<User> GetByToken(string token);

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
