using System;
using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IFormRepository
    {
        Task<Form> Get(string name);
        Task<Form> Get(Guid id);

        Task<IEnumerable<Form>> List();

        Task<IEnumerable<Form>> List(Project project);

        Task<Form> Add(Project project, Form form);

        Task<Form> Delete(Form form);
        Task<Form> Update(Form form);
    }
}
