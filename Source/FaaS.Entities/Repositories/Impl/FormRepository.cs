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
    public class FormRepository : IFormRepository
    {
        private readonly FaaSContext _context;
        private IMapper _mapper;

        public FormRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
        }

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal FormRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
            var config = new MapperConfiguration(cfg => EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public async Task<DataTransferModels.Form> Add(DataTransferModels.Project project, DataTransferModels.Form form)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form dataAccessFormModel = _mapper.Map<Form>(form);

            Project actualProject = _context.Projects.SingleOrDefault(projectForForm => projectForForm.Id == project.Id);
            if (actualProject == null)
            {
                return null;
            }
            dataAccessFormModel.Project = _context.Projects.Find(actualProject.Id);
            dataAccessFormModel.ProjectId = actualProject.Id;

            var addedForm = _context.Forms.Add(dataAccessFormModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Form>(addedForm);
        }

        public async Task<DataTransferModels.Form> Delete(DataTransferModels.Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form oldForm = _context.Forms.SingleOrDefault(formToDelete => formToDelete.Id == form.Id);
            if (oldForm == null)
            {
                return null;
            }

            var deletedForm = _context.Forms.Remove(oldForm);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Form>(deletedForm);
        }

        public async Task<DataTransferModels.Form> Update(DataTransferModels.Form updatedForm)
        {
            if (updatedForm == null)
            {
                throw new ArgumentNullException(nameof(updatedForm));
            }

            Form oldForm = _context.Forms.SingleOrDefault(form => form.Id == updatedForm.Id);
            if (oldForm == null)
            {
                return null;
            }
            Project formProject = _context.Projects.SingleOrDefault(project => project.Id == oldForm.ProjectId);
            oldForm.Project = formProject;

            oldForm.Name = updatedForm.FormName;
            oldForm.Description = updatedForm.Description;
            
            _context.Entry(oldForm).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Form>(oldForm);
        }

        public async Task<DataTransferModels.Form> Get(Guid id)
        {
            Form form = await _context.Forms.SingleOrDefaultAsync(e => e.Id == id);
            Project project = _context.Projects.SingleOrDefault(formProject => formProject.Id == form.ProjectId);
            User user = _context.Users.SingleOrDefault(projectUser => projectUser.Id == project.UserId);
            project.User = user;
            form.Project = project;

            return _mapper.Map<DataTransferModels.Form>(form);
        }


        public async Task<IEnumerable<DataTransferModels.Form>> List()
        {
            var forms = await _context.Forms.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Form>>(forms);
        }

        public async Task<IEnumerable<DataTransferModels.Form>> List(DataTransferModels.Project project)
        {
            var forms = await _context
                        .Forms
                        .Where(form => form.ProjectId == project.Id)
                        .ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Form>>(forms);
        }
    }
}
