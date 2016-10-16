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
    public class ElementValueRepository : IElementValueRepository
    {
        private readonly FaaSContext _context;

        public ElementValueRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<ElementValue> Add(Element element, Session session, ElementValue elementValue)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            if (elementValue == null)
            {
                throw new ArgumentNullException(nameof(elementValue));
            }

            elementValue.Element = _context.Elements.Find(element.Id);
            elementValue.ElementId = element.Id;

            elementValue.Session = _context.Sessions.Find(session.Id);
            elementValue.SessionId = session.Id;

            var addedElementValue = _context.ElementValues.Add(elementValue);
            await _context.SaveChangesAsync();

            return addedElementValue;
        }

        public async Task<ElementValue> Update(ElementValue elementValue)
        {
            if (elementValue == null)
            {
                throw new ArgumentNullException(nameof(elementValue));
            }

            _context.ElementValues.Attach(elementValue);
            _context.Entry(elementValue).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return elementValue;
        }

        public async Task<ElementValue> Delete(ElementValue elementValue)
        {
            if (elementValue == null)
            {
                throw new ArgumentNullException(nameof(elementValue));
            }

            var deletedElementValue = _context.ElementValues.Remove(elementValue);
            await _context.SaveChangesAsync();

            return deletedElementValue;
        }

        public async Task<ElementValue> Get(Guid id)
            => await _context.ElementValues.SingleOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<ElementValue>> List()
            => await _context.ElementValues.ToArrayAsync();

        public async Task<IEnumerable<ElementValue>> List(Session session)
            => await _context
            .ElementValues
            .Where(elementValue => elementValue.SessionId == session.Id)
            .ToArrayAsync();

        public async Task<IEnumerable<ElementValue>> List(Element element)
            => await _context
            .ElementValues
            .Where(elementValue => elementValue.ElementId == element.Id)
            .ToArrayAsync();
    }
}
