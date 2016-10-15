using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetSingleUser(string googleId);

        Task<IEnumerable<User>> List();

        Task<User> Add(User user);

        Task<User> Delete(User user);
    }
}
