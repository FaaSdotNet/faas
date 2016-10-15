using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IElementRepository
    {
        Task<Element> GetSingleElement(string name);

        Task<IEnumerable<Element>> GetAllElements();

        Task<IEnumerable<Element>> GetAllElements(Form form);

        Task<Element> AddElement(Form form, Element element);

        Task<Element> DeleteElement(Element element);
    }
}
