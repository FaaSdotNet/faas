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
        private readonly FaaSContext _context;

        public FormRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
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
        }

        public async Task<Form> Add(Project project, Form form)
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

            var addedForm = _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return addedForm;
        }

        public async Task<Form> Delete(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var deletedForm = _context.Forms.Remove(form);
            await _context.SaveChangesAsync();

            return deletedForm;
        }

        public async Task<Form> Update(Form updatedForm)
        {
            if (updatedForm == null)
            {
                throw new ArgumentNullException(nameof(updatedForm));
            }

            if(_context.Forms.Find(updatedForm.Id) == null)
            {
                throw new ArgumentException("Form not in DB");
            }

            _context.Forms.Attach(updatedForm);
            _context.Entry(updatedForm).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedForm;
        }

        public async Task<Form> Get(Guid id)
            => await _context.Forms.SingleOrDefaultAsync(e => e.Id == id);


        public async Task<IEnumerable<Form>> List()
            => await _context.Forms.ToArrayAsync();

        public async Task<IEnumerable<Form>> List(Project project)
            => await _context
            .Forms
            .Where(form => form.ProjectId == project.Id)
            .ToArrayAsync();

        public async Task<Form> Get(string name)
            => await _context
            .Forms
            .Where(form => form.Name == name)
            .SingleOrDefaultAsync();
    }
}
