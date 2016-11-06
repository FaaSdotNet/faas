using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FaaS.Entities;
using FaaS.Entities.Repositories;
using FaaS.Services.DataTransferModels;

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
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">user repository</param>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public UserService(IUserRepository userRepository, ILogger<IUserService> logger, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.mapper = mapper;
        }


        public async Task<User> Add(User user)
        {
            logger.LogInformation("Add operation called");

            var existingUser = await userRepository.Get(user.GoogleId);
            if (existingUser != null)
            {
                var errorMessage = $"User with Google ID = [{user.GoogleId}] already exists.";
                logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var dataAccesUserModel = mapper.Map<Entities.DataAccessModels.User>(user);
            dataAccesUserModel = await userRepository.Add(dataAccesUserModel);

            return mapper.Map<User>(dataAccesUserModel);
        }

        public async Task<User> Get(string googleId)
        {
            logger.LogInformation("Get [google id] operation called");

            var dataAccessUserModel = await userRepository.Get(googleId);
            return mapper.Map<User>(dataAccessUserModel);
        }

        public async Task<User> Get(Guid id)
        {
            logger.LogInformation("Get [guid] operation called");

            var dataAccessUserModel = await userRepository.Get(id);
            return mapper.Map<User>(dataAccessUserModel);
        }

        public async Task<User[]> GetAll()
        {
            logger.LogInformation("GetAll operation called");

            var dataAccessUserModel = await userRepository.List();
            return mapper.Map<User[]>(dataAccessUserModel);
        }

        public async Task<User> Remove(User user)
        {
            logger.LogInformation("Remove operation called");

            var dataAccessUserModel = mapper.Map<Entities.DataAccessModels.User>(user);
            dataAccessUserModel = await userRepository.Delete(dataAccessUserModel);

            return mapper.Map<User>(dataAccessUserModel);
        }

        public async Task<User> Update(User user)
        {
            logger.LogInformation("Update operation called");

            var dataAccessUserModel = mapper.Map<Entities.DataAccessModels.User>(user);
            dataAccessUserModel = await userRepository.Update(dataAccessUserModel);

            return mapper.Map<User>(dataAccessUserModel);
        }
    }
}
