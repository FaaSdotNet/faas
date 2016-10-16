﻿using FaaS.Entities.DataAccessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IElementValueRepository
    {
        Task<IEnumerable<ElementValue>> List();

        Task<IEnumerable<ElementValue>> List(Element element);

        Task<IEnumerable<ElementValue>> List(Session session);

        Task<ElementValue> Add(Element element, Session session, ElementValue elementValue);
        Task<ElementValue> Update(ElementValue elementValue);


        Task<ElementValue> Delete(ElementValue elementValue);
    }
}
