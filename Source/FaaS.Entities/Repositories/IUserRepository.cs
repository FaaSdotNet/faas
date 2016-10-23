using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(string codeName);

        Task<User> GetGoogle(string googleId);

        Task<User> Get(Guid id);

        Task<IEnumerable<User>> List();

        Task<User> Add(User user);

        Task<User> Add(string googleId, DateTime registered, IEnumerable<Project> projects);

        Task<User> Update(User updatedUser);

        Task<User> Delete(User user);
    }
}
