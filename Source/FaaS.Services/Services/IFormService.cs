using FaaS.Services.DataTransferModels;
using System.Threading.Tasks;
using System;


namespace FaaS.Services
{
    public interface IFormService
    {
        Task<Form> Add(Project project, Form form);

        Task<Form> Get(Project p, string codeName);
        Task<Form> Get(Guid id);


        Task<Form[]> GetAll();

        Task<Form[]> GetAll(Project project);

        Task<Form> Update(Form form);

        Task<Form> Remove(Form form);
    }
}
