using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class ElementRepository : IElementRepository
    {
        public Task<Element> AddElement(Form form, Element element)
        {
            throw new NotImplementedException();
        }

        public Task<Element> DeleteElement(Element element)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Element>> GetAllElements()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Element>> GetAllElements(Form form)
        {
            throw new NotImplementedException();
        }

        public Task<Element> GetSingleElement(string name)
        {
            throw new NotImplementedException();
        }
    }
}
