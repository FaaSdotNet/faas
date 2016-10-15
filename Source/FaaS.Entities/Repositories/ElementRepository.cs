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
        private FaaSContext _context;

        public ElementRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Element> AddElement(Form form, Element element)
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

            Element addedElement = _context.Elements.Add(element);
            await _context.SaveChangesAsync();

            return addedElement;
        }

        public async Task<Element> DeleteElement(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            Element deletedElement = _context.Elements.Remove(element);
            await _context.SaveChangesAsync();

            return deletedElement;
        }

        public async Task<IEnumerable<Element>> GetAllElements()
            => await _context.Elements.ToArrayAsync();

        public async Task<IEnumerable<Element>> GetAllElements(Form form)
            => await _context
            .Elements
            .Where(element => element.FormId == form.Id)
            .ToArrayAsync();

        public async Task<Element> GetSingleElement(string name)
            => await _context
            .Elements
            .Where(element => element.Name == name)
            .SingleOrDefaultAsync();
    }
}
