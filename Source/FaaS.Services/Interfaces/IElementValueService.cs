using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FaaS.DataTransferModels;

namespace FaaS.Services.Interfaces
{
    public interface IElementValueService
    {
        /// <summary>
        /// Adds a new element value to the database
        /// </summary>
        /// <param name="element">element it belongs to</param>
        /// <param name="session">session it belongs to</param>
        /// <param name="elementValue">element value</param>
        /// <returns>Newly added element value</returns>
        Task<ElementValue> Add(Element element, Session session, ElementValue elementValue);

        /// <summary>
        /// Gets element value with given id
        /// </summary>
        /// <param name="id">guid of the element value</param>
        /// <returns>Element value with given id</returns>
        Task<ElementValue> Get(Guid id);

        /// <summary>
        /// Gets all element values belonging to the given element
        /// </summary>
        /// <param name="element">element containing the values</param>
        /// <returns>All element values belonging to the specific element</returns>
        Task<ElementValue[]> GetAllForElement(Element element);

        /// <summary>
        /// Gets all element values belonging to the given session
        /// </summary>
        /// <param name="session">session containing the values</param>
        /// <returns>All element values belonging to the specific session</returns>
        Task<ElementValue[]> GetAllForSession(Session session);

        /// <summary>
        /// Updates the element value
        /// </summary>
        /// <param name="elementValue">element value</param>
        /// <returns>Updated element value</returns>
        Task<ElementValue> Update(ElementValue elementValue);

        /// <summary>
        /// Removes element value
        /// </summary>
        /// <param name="elementValue">element value</param>
        /// <returns>Removed element value</returns>
        Task<ElementValue> Remove(ElementValue elementValue);
    }
}
