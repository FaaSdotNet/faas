using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IOptionRepository
    {
        Task<IEnumerable<Option>> GetAllOptions();

        Task<IEnumerable<Option>> GetAllOptions(Element element);

        Task<Option> AddOption(Element element, Option option);

        Task<Option> DeleteOption(Option option);
    }
}
