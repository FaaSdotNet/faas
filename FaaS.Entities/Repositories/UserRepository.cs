using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    class UserRepository : IUserRepository
    {
        public Task<User> AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetSingleUser(string googleId)
        {
            throw new NotImplementedException();
        }
    }
}
