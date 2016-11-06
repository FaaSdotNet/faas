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
    public class ElementRepository : IElementRepository
    {
        private readonly FaaSContext _context;
        private IMapper _mapper;

        public ElementRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
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
            var config = new MapperConfiguration(cfg => EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public async Task<DataTransferModels.Element> Add(DataTransferModels.Form form, DataTransferModels.Element element)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var dataAccessElementModel = _mapper.Map<Element>(element);

            dataAccessElementModel.Form = _context.Forms.Find(form.Id);
            dataAccessElementModel.FormId = form.Id;

            var addedElement = _context.Elements.Add(dataAccessElementModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.Element>(addedElement);
        }

        public async Task<DataTransferModels.Element> Update(DataTransferModels.Element updatedElement)
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

            return _mapper.Map<DataTransferModels.Element>(oldElement);
        }

        public async Task<DataTransferModels.Element> Delete(DataTransferModels.Element element)
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

            return _mapper.Map<DataTransferModels.Element>(deletedElement);
        }

        public async Task<DataTransferModels.Element> Get(Guid id)
        {
            Element element = await _context.Elements.SingleOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<DataTransferModels.Element>(element);
        }

        public async Task<IEnumerable<DataTransferModels.Element>> GetAll()
        {
            var elements = await _context.Elements.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Element>>(elements);
        }

        public async Task<IEnumerable<DataTransferModels.Element>> GetAll(DataTransferModels.Form form)
        {
            var elements = await _context
                            .Elements
                            .Where(element => element.FormId == form.Id)
                            .ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.Element>>(elements);
        }
    }
}
