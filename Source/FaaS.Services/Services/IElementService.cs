using System;
using FaaS.Services.DataTransferModels;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IElementService
    {
        Task<Element> Add(Form form, Element element);
        Task<Element> Get(Form form, string name);
        Task<Element> Get(Guid id);
        Task<Element[]> GetAll(Form form);
        Task<Element> Update(Element element);
        Task<Element> Remove(Element element);
    }
}