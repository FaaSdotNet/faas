using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class ElementValueRepository : IElementValueRepository
    {
        public Task<ElementValue> AddElementValue(Element element, Session session, ElementValue elementValue)
        {
            throw new NotImplementedException();
        }

        public Task<ElementValue> DeleteElementValue(ElementValue elementValue)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ElementValue>> GetAllElementValues()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ElementValue>> GetAllElementValues(Session session)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ElementValue>> GetAllElementValues(Element element)
        {
            throw new NotImplementedException();
        }
    }
}
