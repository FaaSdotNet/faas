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
    public class OptionRepository : IOptionRepository
    {
        private readonly FaaSContext _context;

        public OptionRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        /// <summary>
        /// Constructor indended for tests' purposes only.
        /// </summary>
        /// <param name="faaSContext">Instance of (eventually mocked) DbContext</param>
        internal OptionRepository(FaaSContext faaSContext)
        {
            if (faaSContext == null)
            {
                throw new ArgumentNullException(nameof(faaSContext));
            }

            _context = faaSContext;
        }

        public async Task<Option> Add(Element element, Option option)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            option.Element = _context.Elements.Find(element.Id);
            option.ElementId = element.Id;

            var addedOption = _context.Options.Add(option);
            await _context.SaveChangesAsync();

            return addedOption;
        }

        public async Task<Option> Delete(Option option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            Option oldOption = _context.Options.SingleOrDefault(optionToDelete => optionToDelete.Id == option.Id);
            if (oldOption == null)
            {
                return null;
            }
            Option deletedOption = _context.Options.Remove(oldOption);

            await _context.SaveChangesAsync();

            return deletedOption;
        }

        public async Task<Option> Update(Option updatedOption)
        {
            if (updatedOption == null)
            {
                throw new ArgumentNullException(nameof(updatedOption));
            }

            Option oldOption = _context.Options.SingleOrDefault(option => option.Id == updatedOption.Id);
            if (oldOption == null)
            {
                throw new ArgumentException("Option not in db!");
            }

            _context.Options.Attach(updatedOption);
            var entry = _context.Entry(updatedOption);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return updatedOption;
        }

        public async Task<Option> Get(Guid id)
            => await _context.Options.SingleOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<Option>> List()
            => await _context.Options.ToArrayAsync();

        public async Task<IEnumerable<Option>> List(Element element)
            => await _context
            .Options
            .Where(option => option.ElementId == element.Id)
            .ToArrayAsync();
    }
}
