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
    public class ElementRepository : IElementRepository
    {
        private readonly FaaSContext _context;

        public ElementRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal ElementRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
        }

        public async Task<Element> Add(Form form, Element element)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.Form = _context.Forms.Find(form.Id);
            element.FormId = form.Id;

            var addedElement = _context.Elements.Add(element);
            await _context.SaveChangesAsync();

            return addedElement;
        }

        public async Task<Element> Update(Element updatedElement)
        {
            if (updatedElement == null)
            {
                throw new ArgumentNullException(nameof(updatedElement));
            }

            Element oldElement = _context.Elements.SingleOrDefault(element => element.Id == updatedElement.Id);
            Form elementForm = _context.Forms.SingleOrDefault(form => form.Id == oldElement.FormId);
            oldElement.Form = elementForm;
            if (oldElement == null)
            {
                return null;
            }

            oldElement.Description = updatedElement.Description;
            oldElement.Required = updatedElement.Required;
            oldElement.Options = updatedElement.Options;
            oldElement.Type = updatedElement.Type;

            _context.Entry(oldElement).State = EntityState.Modified;
         
            await _context.SaveChangesAsync();

            return updatedElement;
        }

        public async Task<Element> Delete(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            Element oldElement = _context.Elements.SingleOrDefault(elementToDelete => elementToDelete.Id == element.Id);
            if (oldElement == null)
            {
                return null;
            }

            var deletedElement = _context.Elements.Remove(oldElement);
            await _context.SaveChangesAsync();

            return deletedElement;
        }

        public async Task<Element> Get(Guid id)
            => await _context.Elements.SingleOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<Element>> GetAll()
            => await _context.Elements.ToArrayAsync();

        public async Task<IEnumerable<Element>> GetAll(Form form)
            => await _context
            .Elements
            .Where(element => element.FormId == form.Id)
            .ToArrayAsync();
    }
}
