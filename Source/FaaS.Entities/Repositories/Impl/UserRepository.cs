using FaaS.Entities.Configuration;
using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
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
        }

        public UserRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<User> Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var addedUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return addedUser;
        }

        public async Task<User> Add(string googleId, DateTime registered, IEnumerable<Project> projects)
        {
            if (googleId == null)
            {
                throw new ArgumentNullException(nameof(googleId));
            }
            if (registered == null)
            {
                throw new ArgumentNullException(nameof(registered));
            }
            if (projects == null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            var user = new User
            {
                GoogleId = googleId,
                Registered = registered
            };

            projects
                .ToList()
                .ForEach(user.Projects.Add);

            var result = await Add(user);
            return result;
        }

        public async Task<User> Update(User updatedUser)
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

            _context.Users.Attach(updatedUser);
            var entry = _context.Entry(updatedUser);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedUser;
        }

        public async Task<User> Delete(User user)
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

            return deletedUser;
        }

        public async Task<User> Get(Guid id)
            => await _context.Users.SingleOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<User>> List()
            => await _context.Users.ToArrayAsync();

        public async Task<User> Get(string name)
            => await _context
            .Users
            .Where(user => user.Name == name)
            .SingleOrDefaultAsync();

        public async Task<User> GetGoogle(string googleId)
            => await _context
            .Users
            .Where(user => user.GoogleId == googleId)
            .SingleOrDefaultAsync();
    }
}
