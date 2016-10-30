using System;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;

namespace FaaS.Services
{
    public interface IUserService
    {
        Task<User> Add(User user);

        Task<User> Get(Guid id);

        Task<User> Get(string googleId);

        Task<User[]> GetAll();

        Task<User> Update(User user);

        Task<User> Remove(User user);
    }
}
