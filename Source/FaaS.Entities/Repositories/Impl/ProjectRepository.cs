using AutoMapper;
using FaaS.Entities.Configuration;
using FaaS.Entities.Contexts;
using FaaS.Entities.DataAccessModels;
using FaaS.Entities.DataAccessModels.Mapping;
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
        private IMapper _mapper;

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
            var config = new MapperConfiguration(cfg => EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public ProjectRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
        }

        public async Task<DataTransferModels.Project> Add(DataTransferModels.User user, DataTransferModels.Project project)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var dataAccessProjectModel = _mapper.Map<Project>(project);

            User actualUser = _context.Users.SingleOrDefault(userForProject => userForProject.Id == user.Id);
            if (actualUser == null)
            {
                return null;
            }
            dataAccessProjectModel.User = _context.Users.Find(actualUser.Id);
            dataAccessProjectModel.UserId = actualUser.Id;

            var addedProject = _context.Projects.Add(dataAccessProjectModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Project>(addedProject);
        }

        public async Task<DataTransferModels.Project> Update(DataTransferModels.Project updatedProject)
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

            User projectUser = _context.Users.SingleOrDefault(user => user.Id == oldProject.UserId);
            oldProject.User = projectUser;

            oldProject.Name = updatedProject.ProjectName;
            oldProject.Description = updatedProject.Description;
            _context.Entry(oldProject).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Project>(oldProject);
        }

        public async Task<DataTransferModels.Project> Delete(DataTransferModels.Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            Project oldProject = _context.Projects.SingleOrDefault(projectToDelete => projectToDelete.Id == project.Id);
            if (oldProject == null)
            {
                return null;
            }

            var deletedProject = _context.Projects.Remove(oldProject);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Project>(deletedProject);
        }

        public async Task<IEnumerable<DataTransferModels.Project>> List()
        {
            var projects = await _context.Projects.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Project>>(projects);
        }

        public async Task<IEnumerable<DataTransferModels.Project>> List(DataTransferModels.User user)
        {
            User actualUser = _context.Users.SingleOrDefault(userForProject => userForProject.Id == user.Id);
            if (actualUser == null)
            {
                return null;
            }
            
            var projects = await _context.Projects
                                .Where(project => project.UserId == actualUser.Id)
                                .ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Project>>(projects);
        }

        public async Task<DataTransferModels.Project> Get(Guid id)
        {
            Project project = await _context.Projects.SingleOrDefaultAsync(e => e.Id == id);
            User user = _context.Users.SingleOrDefault(projectUser => projectUser.Id == project.UserId);
            project.User = user;

            return _mapper.Map<DataTransferModels.Project>(project);
        }
    }
}
