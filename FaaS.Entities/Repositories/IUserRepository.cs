using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetSingleUser(string googleId);

        Task<IEnumerable<User>> GetAllUsers();

        Task<User> AddUser(User user);

        Task<User> DeleteUser(User user);
    }
}
