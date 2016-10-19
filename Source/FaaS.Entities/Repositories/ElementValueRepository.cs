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

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal ElementValueRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
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

            Element elementType = _context.Elements.SingleOrDefault(e => e.Id == element.Id);
            Session elementSession = _context.Sessions.SingleOrDefault(s => s.Id == session.Id);

            if (elementType == null)
            {
                throw new ArgumentException("element not in DB");
            }
            if (elementSession == null)
            {
                throw new ArgumentException("session not in DB");
            }

            elementValue.ElementId = element.Id;
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

            ElementValue oldElementValue = _context.ElementValues.SingleOrDefault(evalue => evalue.Id == elementValue.Id);
            if (oldElementValue == null)
            {
                return null;
            }

            _context.ElementValues.Attach(elementValue);
            var entry = _context.Entry(elementValue);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return elementValue;
        }

        public async Task<ElementValue> Delete(ElementValue elementValue)
        {
            if (elementValue == null)
            {
                throw new ArgumentNullException(nameof(elementValue));
            }

            ElementValue oldElementValue = _context.ElementValues.SingleOrDefault(evalue => evalue.Id == elementValue.Id);
            if (oldElementValue == null)
            {
                return null;
            }

            ElementValue deletedElementValue = _context.ElementValues.Remove(oldElementValue);

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
