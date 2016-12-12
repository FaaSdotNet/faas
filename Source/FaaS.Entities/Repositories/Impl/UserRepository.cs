using AutoMapper;
using FaaS.Entities.Configuration;
using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using FaaS.Entities.DataAccessModels.Mapping;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FaaSContext _context;
        private IMapper _mapper;

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal UserRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
            var config = new MapperConfiguration(cfg => EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public UserRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
        }

        public async Task<DataTransferModels.User> Add(DataTransferModels.User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User dataAccessUserModel = _mapper.Map<User>(user);

            var addedUser = _context.Users.Add(dataAccessUserModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.User>(addedUser);
        }

        public async Task<DataTransferModels.User> Update(DataTransferModels.User updatedUser)
        {
            if (updatedUser == null)
            {
                throw new ArgumentNullException(nameof(updatedUser));
            }

            User oldUser = _context.Users.SingleOrDefault(user => user.Id == updatedUser.Id);
            if (oldUser == null)
            {
                throw new ArgumentException("User not in db!");
            }

            oldUser.Name = updatedUser.Name;
            oldUser.GoogleToken = updatedUser.GoogleToken;
            _context.Entry(oldUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.User>(oldUser);
        }

        public async Task<DataTransferModels.User> Delete(DataTransferModels.User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User oldUser = _context.Users.SingleOrDefault(userToDelete => userToDelete.Id == user.Id);
            if (oldUser == null)
            {
                throw new ArgumentException("User not in db!");
            }

            User deletedUser = _context.Users.Remove(oldUser);

            await _context.SaveChangesAsync();

            return _mapper.Map< DataTransferModels.User>(deletedUser);
        }

        public async Task<DataTransferModels.User> Get(Guid id)
        {
            User user = await _context.Users.SingleOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<DataTransferModels.User>(user);
        }

        public async Task<DataTransferModels.User> Get(string email)
        {
            User user = await _context
                                .Users
                                .Where(u => u.Email == email)
                                .SingleOrDefaultAsync();

            return _mapper.Map<DataTransferModels.User>(user);
        }


        public async Task<DataTransferModels.User> GetByToken(string token)
        {
            User user = await _context
                                .Users
                                .Where(u => u.GoogleToken == token)
                                .SingleOrDefaultAsync();

            return _mapper.Map<DataTransferModels.User>(user);
        }

        public async Task<IEnumerable<DataTransferModels.User>> List()
        {
            var users = await _context.Users.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.User>>(users);
        }
    }
}
