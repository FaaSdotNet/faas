using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        public Task<Option> AddOption(Element element, Option option)
        {
            throw new NotImplementedException();
        }

        public Task<Option> DeleteOption(Option option)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Option>> GetAllOptions()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Option>> GetAllOptions(Element element)
        {
            throw new NotImplementedException();
        }
    }
}
