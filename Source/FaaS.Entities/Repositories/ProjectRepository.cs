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

            project.User = _context.Users.Find(user.Id);
            project.UserId = user.Id;

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

            updatedProject = _context.Projects.Attach(updatedProject);
            _context.Entry(updatedProject).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedProject;
        }

        public async Task<Project> Delete(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var deletedProject = _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return deletedProject;
        }
       
        public async Task<IEnumerable<Project>> List()
            => await _context.Projects.ToArrayAsync();

        public async Task<IEnumerable<Project>> List(User user)
            => await _context
            .Projects
            .Where(project => project.UserId == user.Id)
            .ToArrayAsync();


        public async Task<Project> Get(string name)
            => await _context
            .Projects
            .Where(project => project.Name == name)
            .SingleOrDefaultAsync();
        public async Task<Project> Get(long id)
          => await _context.Projects.FindAsync(id);
    }
}
