using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IElementValueRepository
    {
        Task<IEnumerable<ElementValue>> GetAllElementValues();

        Task<IEnumerable<ElementValue>> GetAllElementValues(Element element);

        Task<IEnumerable<ElementValue>> GetAllElementValues(Session session);

        Task<ElementValue> AddElementValue(Element element, Session session, ElementValue elementValue);

        Task<ElementValue> DeleteElementValue(ElementValue elementValue);
    }
}
