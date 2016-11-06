using FaaS.DataTransferModels;
using System;
using System.Threading.Tasks;

namespace FaaS.Services
{
    public interface IFaaSService
    {
        Task<User> AddUser(User user);

        Task<User> GetUser(Guid id);

        Task<User> GetUser(string googleId);

        Task<User[]> GetAllUsers();

        Task<User> UpdateUser(User user);

        Task<User> RemoveUser(User user);

        Task<Project> AddProject(User user, Project project);

        Task<Project> GetProject(Guid id);

        Task<Project[]> GetAllProjects(User user);

        Task<Project> UpdateProject(Project project);

        Task<Project> RemoveProject(Project project);

        Task<Form> AddForm(Project project, Form form);

        Task<Form> GetForm(Guid id);

        Task<Form[]> GetAllForms();

        Task<Form[]> GetAllForms(Project project);

        Task<Form> UpdateForm(Form form);

        Task<Form> RemoveForm(Form form);

        Task<Element> AddElement(Form form, Element element);

        Task<Element> GetElement(Guid id);

        Task<Element[]> GetAllElements();

        Task<Element[]> GetAllElements(Form form);

        Task<Element> UpdateElement(Element element);

        Task<Element> RemoveElement(Element element);

        Task<ElementValue> AddElementValue(Element element, Session session, ElementValue elementValue);

        Task<ElementValue> GetElementValue(Guid id);

        Task<ElementValue[]> GetAllElementValues(Element element);

        Task<ElementValue[]> GetAllElementValues(Session session);

        Task<ElementValue> UpdateElementValue(ElementValue element);

        Task<ElementValue> RemoveElementValue(ElementValue element);

        Task<Session> AddSession(Session session);

        Task<Session> GetSession(Guid id);

        Task<Session[]> GetAllSessions();

        Task<Session> UpdateSession(Session session);

        Task<Session> RemoveSession(Session session);
    }
}
