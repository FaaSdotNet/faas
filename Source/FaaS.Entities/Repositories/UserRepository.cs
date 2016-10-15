﻿using FaaS.Entities.Configuration;
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
    class UserRepository : IUserRepository
    {
        private FaaSContext _context;

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal UserRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
                throw new ArgumentNullException(nameof(faaSContext));

            _context = faaSContext;
        }

        public UserRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<User> AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User addedUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return addedUser;
        }

        public async Task<User> AddUser(string googleId, DateTime registered, IEnumerable<Project> projects)
        {
            if (googleId == null)
                throw new ArgumentNullException(nameof(googleId));
            if (registered == null)
                throw new ArgumentNullException(nameof(registered));
            if (projects == null)
                throw new ArgumentNullException(nameof(projects));

            User user = new User
            {
                GoogleId = googleId,
                Registered = registered
            };

            projects
                .ToList()
                .ForEach(user.Projects.Add);

            var addedUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return addedUser;
        }

        public async Task<User> DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User deletedUser = _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return deletedUser;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
            => await _context.Users.ToArrayAsync();

        public async Task<User> GetSingleUser(string googleId)
            => await _context
            .Users
            .Where(user => user.GoogleId == googleId)
            .SingleOrDefaultAsync();
    }
}
