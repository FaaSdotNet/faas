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
    public class ProjectRepository : IProjectRepository
    {
        private readonly FaaSContext _context;
        
        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal ProjectRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
        }

        public ProjectRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Project> Add(User user, Project project)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            User actualUser = _context.Users.SingleOrDefault(userForProject => userForProject.GoogleId == user.GoogleId);
            if (actualUser == null)
            {
                return null;
            }
            project.User = _context.Users.Find(actualUser.Id);
            project.UserId = actualUser.Id;

            var addedProject = _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return addedProject;
        }

        public async Task<Project> Update(Project updatedProject)
        {
            if (updatedProject == null)
            {
                throw new ArgumentNullException(nameof(updatedProject));
            }

            Project oldProject = _context.Projects.SingleOrDefault(project => project.Id == updatedProject.Id);
            if (oldProject == null)
            {
                throw new ArgumentException("Project not in DB");
            }

            _context.Projects.Attach(updatedProject);
            var entry = _context.Entry(updatedProject);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedProject;
        }

        public async Task<Project> Delete(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Project oldProject = _context.Projects.SingleOrDefault
                (projectToDelete => projectToDelete.Name == project.Name && projectToDelete.UserId == project.UserId);
            if (oldProject == null)
            {
                return null;
            }

            var deletedProject = _context.Projects.Remove(oldProject);
            await _context.SaveChangesAsync();

            return deletedProject;
        }
       
        public async Task<IEnumerable<Project>> List()
            => await _context.Projects.ToArrayAsync();

        public async Task<IEnumerable<Project>> List(User user)
        {
            User actualUser = _context.Users.SingleOrDefault(
                userForProject => userForProject.GoogleId == user.GoogleId
                );
            if (actualUser == null)
            {
                return null;
            }

            return await _context.Projects
            .Where(project => project.UserId == actualUser.Id)
            .ToArrayAsync();
        }

        public async Task<Project> Get(Guid userId, string name)
            => await _context
            .Projects
            .Where(project => project.Name == name && userId == project.UserId)
            .SingleOrDefaultAsync();

        public async Task<Project> Get(User user, string name) => 
            await this.Get(user.Id, name);


        public async Task<Project> Get(Guid id)
          => await _context.Projects.SingleOrDefaultAsync(e => e.Id == id);
    }
}
