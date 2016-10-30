using System;
using FaaS.Services.DataTransferModels;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IElementValueService
    {
        Task<ElementValue> Add(Element element, Session session, ElementValue elementValue);

        Task<ElementValue> Get(Guid id);

        Task<ElementValue[]> GetAll(Element element);

        Task<ElementValue[]> GetAll(Session session);

        Task<ElementValue> Update(ElementValue element);

        Task<ElementValue> Remove(ElementValue element);

    }
}