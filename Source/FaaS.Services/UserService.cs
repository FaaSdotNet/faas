using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;
using FaaS.DataTransferModels;

namespace FaaS.Services
{
    /// <summary>
    /// Implementation of <see cref="IUserService"/> interface
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IUserService> logger;

        /// <summary>
        /// User repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">user repository</param>
        /// <param name="logger">logger</param>
        public UserService(IUserRepository userRepository, ILogger<IUserService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<User> Add(User user)
        {
            logger.LogInformation("Add operation called");

            var existingUser = await userRepository.Get(user.GoogleId);
            if (existingUser == null)
                return await userRepository.Add(user);

            var errorMessage = $"User with Google ID = [{user.GoogleId}] already exists.";
            logger.LogError(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }

        public async Task<User> Get(string googleId)
        {
            logger.LogInformation("Get [google id] operation called");

            return await userRepository.Get(googleId);
        }

        public async Task<User> Get(Guid id)
        {
            logger.LogInformation("Get [guid] operation called");

            return await userRepository.Get(id);
        }

        public async Task<User[]> GetAll()
        {
            logger.LogInformation("GetAll operation called");

            return (await userRepository.List()).ToArray();
        }

        public async Task<User> Remove(User user)
        {
            logger.LogInformation("Remove operation called");

            return await userRepository.Delete(user);
        }

        public async Task<User> Update(User user)
        {
            logger.LogInformation("Update operation called");

            return await userRepository.Update(user);
        }
    }
}
