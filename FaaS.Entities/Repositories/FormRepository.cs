using FaaS.Entities.DataAccessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public class FormRepository : IFormRepository
    {
        public Task<Form> AddForm(Project project, Form form)
        {
            throw new NotImplementedException();
        }

        public Task<Form> DeleteForm(Form form)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Form>> GetAllForms()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Form>> GetAllForms(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Form> GetSingleForm(string name)
        {
            throw new NotImplementedException();
        }
    }
}
