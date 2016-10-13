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
        private FaaSContext _context;

        public OptionRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
        }

        public async Task<Option> AddOption(Element element, Option option)
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

            Option addedOption = _context.Options.Add(option);
            await _context.SaveChangesAsync();

            return addedOption;
        }

        public async Task<Option> DeleteOption(Option option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            Option deletedOption = _context.Options.Remove(option);
            await _context.SaveChangesAsync();

            return deletedOption;
        }

        public async Task<IEnumerable<Option>> GetAllOptions()
            => await _context.Options.ToArrayAsync();

        public async Task<IEnumerable<Option>> GetAllOptions(Element element)
            => await _context
            .Options
            .Where(option => option.ElementId == element.Id)
            .ToArrayAsync();
    }
}
