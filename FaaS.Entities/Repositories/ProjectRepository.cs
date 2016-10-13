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
        private FaaSContext _context;

        public ProjectRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Project> AddProject(User user, Project project)
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

            Project addedProject = _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return addedProject;
        }

        public async Task<Project> DeleteProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Project deletedProject = _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return deletedProject;
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
            => await _context.Projects.ToArrayAsync();

        public async Task<IEnumerable<Project>> GetAllProjects(User user)
            => await _context
            .Projects
            .Where(project => project.UserId == user.Id)
            .ToArrayAsync();


        public async Task<Project> GetSingleProject(string name)
            => await _context
            .Projects
            .Where(project => project.Name == name)
            .SingleOrDefaultAsync();
    }
}
