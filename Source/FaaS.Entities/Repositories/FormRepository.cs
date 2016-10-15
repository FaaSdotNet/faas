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
    public class FormRepository : IFormRepository
    {
        private FaaSContext _context;

        public FormRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Form> AddForm(Project project, Form form)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            form.Project = _context.Projects.Find(project.Id);
            form.ProjectId = project.Id;

            Form addedForm = _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return addedForm;
        }

        public async Task<Form> DeleteForm(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Form deletedForm = _context.Forms.Remove(form);
            await _context.SaveChangesAsync();

            return deletedForm;
        }

        public async Task<IEnumerable<Form>> GetAllForms()
            => await _context.Forms.ToArrayAsync();

        public async Task<IEnumerable<Form>> GetAllForms(Project project)
            => await _context
            .Forms
            .Where(form => form.ProjectId == project.Id)
            .ToArrayAsync();

        public async Task<Form> GetSingleForm(string name)
            => await _context
            .Forms
            .Where(form => form.Name == name)
            .SingleOrDefaultAsync();
    }
}
