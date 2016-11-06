using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;
using FaaS.DataTransferModels;
using System.Linq;

namespace FaaS.Services
{
    public class FaaSService : IFaaSService
    {
        private readonly ILogger<IFaaSService> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IFormRepository _formRepository;
        private readonly IElementRepository _elementRepository;
        private readonly IElementValueRepository _elementValueRepository;
        private readonly ISessionRepository _sessionRepository;

        public FaaSService(
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IFormRepository formRepository,
            IElementRepository elementRepository,
            IElementValueRepository elementValueRepository,
            ISessionRepository sessionRepository,
            ILogger<IFaaSService> logger)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _formRepository = formRepository;
            _elementRepository = elementRepository;
            _elementValueRepository = elementValueRepository;
            _sessionRepository = sessionRepository;

            _logger = logger;
        }

        public async Task<Element> AddElement(Form form, Element element)
        {
            _logger.LogInformation("AddElement");

            var existingForm = await _formRepository.Get(form.Id);
            if (existingForm == null)
            {
                var message = $"Form with id {form.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingElement = await _elementRepository.Get(element.Id);
            if (existingElement != null)
            {
                var message = $"Element with id {element.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferElementModel = await _elementRepository.Add(existingForm, element);

            return dataTransferElementModel;
        }

        public async Task<ElementValue> AddElementValue(Element element, Session session, ElementValue elementValue)
        {
            _logger.LogInformation("AddElementValue");

            var existingElement = await _elementRepository.Get(element.Id);
            if (existingElement == null)
            {
                var message = $"Element with id {element.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingSession = await _sessionRepository.Get(session.Id);
            if (existingSession == null)
            {
                var message = $"Session with id {session.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingElementValue = await _elementValueRepository.Get(elementValue.Id);
            if (existingElementValue != null)
            {
                var message = $"Element value with id {elementValue.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferElementValueModel = await _elementValueRepository.Add(existingElement, existingSession, elementValue);

            return dataTransferElementValueModel;
        }

        public async Task<Form> AddForm(Project project, Form form)
        {
            _logger.LogInformation("AddForm");

            var existingProject = await _projectRepository.Get(project.Id);
            if (existingProject == null)
            {
                var message = $"Project with id {project.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingForm = await _formRepository.Get(form.Id);
            if (existingForm != null)
            {
                var message = $"Form with id {form.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferFormModel = await _formRepository.Add(existingProject, form);

            return dataTransferFormModel;
        }

        public async Task<Project> AddProject(User user, Project project)
        {
            _logger.LogInformation("AddProject");

            var existingUser = await _userRepository.Get(user.GoogleId);
            if (existingUser == null)
            {
                var message = $"User with Google ID {user.GoogleId} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var existingProject = await _projectRepository.Get(project.Id);
            if (existingProject != null)
            {
                var message = $"Project with id {project.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferProjectModel = await _projectRepository.Add(existingUser, project);

            return dataTransferProjectModel;
        }

        public async Task<Session> AddSession(Session session)
        {
            _logger.LogInformation("AddSession");

            var existingSession = await _sessionRepository.Get(session.Id);
            if (existingSession != null)
            {
                var message = $"Session with id {session.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferSessionModel = await _sessionRepository.Add(session);

            return dataTransferSessionModel;
        }

        public async Task<User> AddUser(User user)
        {
            _logger.LogInformation("AddUser");

            var existingUser = await _userRepository.Get(user.GoogleId);
            if (existingUser != null)
            {
                var message = $"User with Google ID {user.GoogleId} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferUserModel = await _userRepository.Add(user);

            return dataTransferUserModel;
        }

        public async Task<Element[]> GetAllElements()
        {
            _logger.LogInformation("GetAllElements");

            var dataTransferElementModel = await _elementRepository.GetAll();

            return dataTransferElementModel.ToArray();
        }

        public async Task<Element[]> GetAllElements(Form form)
        {
            _logger.LogInformation("GetAllElements for form");

            var existingForm = await _formRepository.Get(form.Id);
            if (existingForm == null)
            {
                var message = $"Form with id {form.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferElementModel = await _elementRepository.GetAll(existingForm);

            return dataTransferElementModel.ToArray();
        }

        public async Task<ElementValue[]> GetAllElementValues(Session session)
        {
            _logger.LogInformation("GetAllElementValues for session");

            var existingSession = await _sessionRepository.Get(session.Id);
            if (existingSession == null)
            {
                var message = $"Session with id {session.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferElementValueModel = await _elementValueRepository.List(existingSession);

            return dataTransferElementValueModel.ToArray();
        }

        public async Task<ElementValue[]> GetAllElementValues(Element element)
        {
            _logger.LogInformation("GetAllElementValues for element");

            var existingElement = await _elementRepository.Get(element.Id);
            if (existingElement == null)
            {
                var message = $"Element with id {element.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferElementValueModel = await _elementValueRepository.List(existingElement);

            return dataTransferElementValueModel.ToArray();
        }

        public async Task<Form[]> GetAllForms()
        {
            _logger.LogInformation("GetAllForms");

            var dataTransferFormModel = await _formRepository.List();

            return dataTransferFormModel.ToArray();
        }

        public async Task<Form[]> GetAllForms(Project project)
        {
            _logger.LogInformation("GetAllForms for project");

            var existingProject = await _projectRepository.Get(project.Id);
            if (existingProject == null)
            {
                var message = $"Project with id {project.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferFormModel = await _formRepository.List(existingProject);

            return dataTransferFormModel.ToArray();
        }

        public async Task<Project[]> GetAllProjects(User user)
        {
            _logger.LogInformation("GetAllProjects for user");

            var existingUser = await _userRepository.Get(user.GoogleId);
            if (existingUser == null)
            {
                var message = $"User with Google ID {user.GoogleId} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            var dataTransferProjectModel = await _projectRepository.List(existingUser);

            return dataTransferProjectModel.ToArray();
        }

        public async Task<Session[]> GetAllSessions()
        {
            _logger.LogInformation("GetAllSessions");

            var dataTransferSessionModel = await _sessionRepository.List();
            return dataTransferSessionModel.ToArray();
        }

        public async Task<User[]> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers");

            var dataTransferUserModel = await _userRepository.List();
            return dataTransferUserModel.ToArray();
        }

        public async Task<Element> GetElement(Guid id)
        {
            _logger.LogInformation("GetElement");
            
            var dataTransferElementModel = await _elementRepository.Get(id);

            return dataTransferElementModel;
        }

        public async Task<ElementValue> GetElementValue(Guid id)
        {
            _logger.LogInformation("GetElementValue");

            var dataTransferElementValueModel = await _elementValueRepository.Get(id);

            return dataTransferElementValueModel;
        }

        public async Task<Form> GetForm(Guid id)
        {
            _logger.LogInformation("GetForm");

            var dataTransferFormModel = await _formRepository.Get(id);

            return dataTransferFormModel;
        }

        public async Task<Project> GetProject(Guid id)
        {
            _logger.LogInformation("GetProject");

            var dataTransferProjectModel = await _projectRepository.Get(id);

            return dataTransferProjectModel;
        }

        public async Task<Session> GetSession(Guid id)
        {
            _logger.LogInformation("GetSession");

            Session dataTransferSessionModel = await _sessionRepository.Get(id);

            return dataTransferSessionModel;
        }

        public async Task<User> GetUser(Guid id)
        {
            _logger.LogInformation("GetUser");

            User dataTransferUserModel = await _userRepository.Get(id);

            return dataTransferUserModel;
        }

        public async Task<User> GetUser(string googleId)
        {
            _logger.LogInformation("GetUser with Google ID");

            User dataTransferUserModel = await _userRepository.Get(googleId);

            return dataTransferUserModel;
        }

        public async Task<Element> RemoveElement(Element element)
        {
            _logger.LogInformation("RemoveElement");

            var dataTransferElementModel = await _elementRepository.Delete(element);

            return dataTransferElementModel;
        }

        public async Task<ElementValue> RemoveElementValue(ElementValue elementValue)
        {
            _logger.LogInformation("RemoveElementValue");

            var dataTransferElementValueModel = await _elementValueRepository.Delete(elementValue);

            return dataTransferElementValueModel;
        }

        public async Task<Form> RemoveForm(Form form)
        {
            _logger.LogInformation("RemoveForm");

            var dataTransferFormModel = await _formRepository.Delete(form);

            return dataTransferFormModel;
        }

        public async Task<Project> RemoveProject(Project project)
        {
            _logger.LogInformation("RemoveProject");

            Project dataTransferProjectModel = await _projectRepository.Delete(project);

            return dataTransferProjectModel;
        }

        public async Task<Session> RemoveSession(Session session)
        {
            _logger.LogInformation("RemoveSession");

            var dataTransferSessionModel = await _sessionRepository.Delete(session);

            return dataTransferSessionModel;
        }

        public async Task<User> RemoveUser(User user)
        {
            _logger.LogInformation("RemoveUser");

            User dataTransferUserModel = await _userRepository.Delete(user);

            return dataTransferUserModel;
        }

        public async Task<Element> UpdateElement(Element element)
        {
            _logger.LogInformation("UpdateElement");

            Element dataTransferElementModel = await _elementRepository.Update(element);

            return dataTransferElementModel;
        }

        public async Task<ElementValue> UpdateElementValue(ElementValue elementValue)
        {
            _logger.LogInformation("UpdateElementValue");

            var dataTransferElementValueModel = await _elementValueRepository.Update(elementValue);

            return dataTransferElementValueModel;
        }

        // updating description and display name only
        public async Task<Form> UpdateForm(Form form)
        {
            _logger.LogInformation("UpdateForm");

            var dataTransferFormModel = await _formRepository.Update(form);

            return dataTransferFormModel;
        }

        public async Task<Project> UpdateProject(Project project)
        {
            _logger.LogInformation("UpdateProject");

            var dataTransferProjectModel = await _projectRepository.Update(project);

            return dataTransferProjectModel;
        }

        public async Task<Session> UpdateSession(Session session)
        {
            _logger.LogInformation("UpdateSession");

            var dataTransferSessionModel = await _sessionRepository.Update(session);

            return dataTransferSessionModel;
        }

        public async Task<User> UpdateUser(User user)
        {
            _logger.LogInformation("UpdateUser");

            var dataTransferUserModel = await _userRepository.Update(user);

            return dataTransferUserModel;
        }
    }
}
