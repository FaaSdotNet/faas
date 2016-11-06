using FaaS.Services.DataTransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IFormService
    {
        Task<Form> Add(Project project, Form form);

        Task<Form> Get(Guid id);

        Task<Form[]> GetAll();

        Task<Form[]> GetAllForProject(Project project);

        Task<Form> Update(Form form);

        Task<Form> Remove(Form form);
    }
}
