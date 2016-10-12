using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IFormRepository
    {
        Task<Form> GetSingleForm(string name);

        Task<IEnumerable<Form>> GetAllForms();

        Task<IEnumerable<Form>> GetAllForms(Project project);

        Task<Form> AddForm(Project project, Form form);

        Task<Form> DeleteForm(Form form);
    }
}
