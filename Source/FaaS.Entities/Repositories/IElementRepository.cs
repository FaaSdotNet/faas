﻿using System;
using FaaS.DataTransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.Entities.Repositories
{
    public interface IElementRepository
    {
        Task<Element> Get(Guid id);

        Task<IEnumerable<Element>> GetAll();

        Task<IEnumerable<Element>> GetAll(Form form);

        Task<Element> Add(Form form, Element element);

        Task<Element> Update(Element element);

        Task<Element> Delete(Element element);
    }
}
