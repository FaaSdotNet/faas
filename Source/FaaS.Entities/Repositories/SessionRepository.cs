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
    public class SessionRepository : ISessionRepository
    {
        private readonly FaaSContext _context;

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal SessionRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
        }

        public SessionRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Session> Add(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var addedSession = _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return addedSession;
        }

        public async Task<Session> Delete(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var deletedSession = _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return deletedSession;
        }

        public async Task<IEnumerable<Session>> List()
            => await _context.Sessions.ToArrayAsync();

        public async Task<Session> Update(Session updatedSession)
        {
            if (updatedSession == null)
            {
                throw new ArgumentNullException(nameof(updatedSession));
            }

            updatedSession =_context.Sessions.Attach(updatedSession);
            _context.Entry(updatedSession).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedSession;
        }

        public async Task<Session> Get(long id) =>
            await _context.Sessions.FindAsync(id);

    }
}
