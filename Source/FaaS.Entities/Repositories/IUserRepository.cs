using FaaS.DataTransferModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);

        Task<User> Get(string email);

        Task<User> GetByToken(string token);

        Task<IEnumerable<User>> List();

        Task<User> Add(User user);

        Task<User> Update(User updatedUser);

        Task<User> Delete(User user);
    }
}
