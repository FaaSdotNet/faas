using System;
using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IOptionRepository
    {
        Task<IEnumerable<Option>> List();

        Task<IEnumerable<Option>> List(Element element);

        Task<Option> Add(Element element, Option option);

        Task<Option> Delete(Option option);
        Task<Option> Update(Option option);
        Task<Option> Get(Guid id);
    }
}
