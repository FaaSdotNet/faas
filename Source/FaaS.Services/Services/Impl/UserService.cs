using System;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Entities.Repositories;
using FaaS.Services.DataTransferModels;
using Microsoft.Extensions.Logging;

namespace FaaS.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private readonly ILogger<IUserService> _logger;


        public UserService(
            IUserRepository userRepository,
            ILogger<IUserService> logger,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;

        }

        public Task<User> Add(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(string googleId)
        {
            throw new NotImplementedException();
        }

        public Task<User[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> Remove(User user)
        {
            throw new NotImplementedException();
        }
    }
}