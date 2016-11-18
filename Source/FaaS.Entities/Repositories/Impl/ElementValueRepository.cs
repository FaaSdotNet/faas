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
    public class ElementValueRepository : IElementValueRepository
    {
        private readonly FaaSContext _context;
        private IMapper _mapper;

        public ElementValueRepository(IOptions<ConnectionOptions> connectionOptions, IMapper mapper)
        {
            _context = new FaaSContext(connectionOptions.Value.ConnectionString);
            _mapper = mapper;
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
            var config = new MapperConfiguration(cfg =>
                EntitiesMapperConfiguration.InitializeMappings(cfg));
            _mapper = config.CreateMapper();
        }

        public async Task<DataTransferModels.ElementValue> Add(DataTransferModels.Element element, DataTransferModels.Session session, DataTransferModels.ElementValue elementValue)
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
            elementType.Form = _context.Forms.SingleOrDefault(f => f.Id == elementType.FormId);
            Session elementSession = _context.Sessions.SingleOrDefault(s => s.Id == session.Id);

            if (elementType == null)
            {
                throw new ArgumentException("element not in DB");
            }
            if (elementSession == null)
            {
                throw new ArgumentException("session not in DB");
            }

            ElementValue dataAccessElementValueModel = _mapper.Map<ElementValue>(elementValue);

            dataAccessElementValueModel.ElementId = element.Id;
            dataAccessElementValueModel.Element = elementType;
            dataAccessElementValueModel.SessionId = session.Id;

            var addedElementValue = _context.ElementValues.Add(dataAccessElementValueModel);
            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.ElementValue>(addedElementValue);
        }

        public async Task<DataTransferModels.ElementValue> Update(DataTransferModels.ElementValue elementValue)
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

            oldElementValue.Value = elementValue.Value;
            _context.Entry(oldElementValue).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<DataTransferModels.ElementValue>(oldElementValue);
        }

        public async Task<DataTransferModels.ElementValue> Delete(DataTransferModels.ElementValue elementValue)
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

            return _mapper.Map<DataTransferModels.ElementValue>(deletedElementValue);
        }

        public async Task<DataTransferModels.ElementValue> Get(Guid id)
        {
            ElementValue elementValue = await _context.ElementValues
                                                .SingleOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<DataTransferModels.ElementValue>(elementValue);
        }

        public async Task<IEnumerable<DataTransferModels.ElementValue>> List()
        {
            var elementValues = await _context.ElementValues.ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.ElementValue>>(elementValues);
        }

        public async Task<IEnumerable<DataTransferModels.ElementValue>> List(DataTransferModels.Session session)
        {
            var elementValues = await _context
                                    .ElementValues
                                    .Where(elementValue => elementValue.SessionId == session.Id)
                                    .ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.ElementValue>>(elementValues);
        }

        public async Task<IEnumerable<DataTransferModels.ElementValue>> List(DataTransferModels.Element element)
        {
            var elementValues = await _context
                                    .ElementValues
                                    .Where(elementValue => elementValue.ElementId == element.Id)
                                    .ToArrayAsync();

            return _mapper.Map<IEnumerable<DataTransferModels.ElementValue>>(elementValues);
        }
    }
}
