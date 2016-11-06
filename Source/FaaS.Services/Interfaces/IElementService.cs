using FaaS.Services.DataTransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IElementService
    {
        /// <summary>
        /// Adds a new element to the database
        /// </summary>
        /// <param name="form"></param>
        /// <param name="element">new element</param>
        /// <returns>Newly created element</returns>
        Task<Element> Add(Form form, Element element);

        /// <summary>
        /// Gets element with given id
        /// </summary>
        /// <param name="id">guid of the element</param>
        /// <returns>element with given id</returns>
        Task<Element> Get(Guid id);

        /// <summary>
        /// Gets all elements
        /// </summary>
        /// <returns>All elements</returns>
        Task<Element[]> GetAll();

        /// <summary>
        /// Gets all elements contained in a given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>All elements belonging to given form</returns>
        Task<Element[]> GetAllForForm(Form form);

        /// <summary>
        /// Updates the element
        /// </summary>
        /// <param name="element">element to be updated</param>
        /// <returns>updated element</returns>
        Task<Element> Update(Element element);

        /// <summary>
        /// Removes the element
        /// </summary>
        /// <param name="element">element to be removed</param>
        /// <returns>Removed element</returns>
        Task<Element> Remove(Element element);
    }
}
