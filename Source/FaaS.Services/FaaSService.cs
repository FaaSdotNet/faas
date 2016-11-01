using System;
using System.Threading.Tasks;
using FaaS.Entities.DataAccessModels;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;
using AutoMapper;

namespace FaaS.Services
{
    public class FaaSService : IFaaSService
    {
        private readonly ILogger<IFaaSService> _logger;
        private IMapper _mapper;

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
            ILogger<IFaaSService> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _formRepository = formRepository;
            _elementRepository = elementRepository;
            _elementValueRepository = elementValueRepository;
            _sessionRepository = sessionRepository;

            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DataTransferModels.Element> AddElement(DataTransferModels.Form form, DataTransferModels.Element element)
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

            Form dataAccessFormModel = _mapper.Map<Form>(existingForm);
            Element dataAccessElementModel = _mapper.Map<Element>(element);
            dataAccessElementModel = await _elementRepository.Add(dataAccessFormModel, dataAccessElementModel);

            return _mapper.Map<DataTransferModels.Element>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.ElementValue> AddElementValue(DataTransferModels.Element element, DataTransferModels.Session session, DataTransferModels.ElementValue elementValue)
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

            Element dataAccessElementModel = _mapper.Map<Element>(element);
            Session dataAccessSessionModel = _mapper.Map<Session>(session);
            ElementValue dataAccessElementValueModel = _mapper.Map<ElementValue>(elementValue);
            dataAccessElementValueModel = await _elementValueRepository.Add(dataAccessElementModel, dataAccessSessionModel, dataAccessElementValueModel);

            return _mapper.Map<DataTransferModels.ElementValue>(dataAccessElementValueModel);
        }

        public async Task<DataTransferModels.Form> AddForm(DataTransferModels.Project project, DataTransferModels.Form form)
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

            Project dataAccessProjectModel = _mapper.Map<Project>(project);
            Form dataAccessFormModel = _mapper.Map<Form>(form);
            dataAccessFormModel = await _formRepository.Add(dataAccessProjectModel, dataAccessFormModel);

            return _mapper.Map<DataTransferModels.Form>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Project> AddProject(DataTransferModels.User user, DataTransferModels.Project project)
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

            User dataAccessUserModel = _mapper.Map<User>(user);
            Project dataAccessProjectModel = _mapper.Map<Project>(project);
            dataAccessProjectModel = await _projectRepository.Add(dataAccessUserModel, dataAccessProjectModel);

            return _mapper.Map<DataTransferModels.Project>(dataAccessProjectModel);
        }

        public async Task<DataTransferModels.Session> AddSession(DataTransferModels.Session session)
        {
            _logger.LogInformation("AddSession");

            var existingSession = await _sessionRepository.Get(session.Id);
            if (existingSession != null)
            {
                var message = $"Session with id {session.Id} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            Session dataAccessSessionModel = _mapper.Map<Session>(session);
            dataAccessSessionModel = await _sessionRepository.Add(dataAccessSessionModel);

            return _mapper.Map<DataTransferModels.Session>(dataAccessSessionModel);
        }

        public async Task<DataTransferModels.User> AddUser(DataTransferModels.User user)
        {
            _logger.LogInformation("AddUser");

            var existingUser = await _userRepository.Get(user.GoogleId);
            if (existingUser != null)
            {
                var message = $"User with Google ID {user.GoogleId} already exists.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            User dataAccessUserModel = _mapper.Map<User>(user);
            dataAccessUserModel = await _userRepository.Add(dataAccessUserModel);

            return _mapper.Map<DataTransferModels.User>(dataAccessUserModel);
        }

        public async Task<DataTransferModels.Element[]> GetAllElements()
        {
            _logger.LogInformation("GetAllElements");

            var dataAccessElementModel = await _elementRepository.GetAll();

            return _mapper.Map<DataTransferModels.Element[]>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.Element[]> GetAllElements(DataTransferModels.Form form)
        {
            _logger.LogInformation("GetAllElements for form");

            var existingForm = await _formRepository.Get(form.Id);
            if (existingForm == null)
            {
                var message = $"Form with id {form.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            Form dataAccessFormModel = _mapper.Map<Form>(existingForm);
            var dataAccessElementModel = await _elementRepository.GetAll(dataAccessFormModel);

            return _mapper.Map<DataTransferModels.Element[]>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.ElementValue[]> GetAllElementValues(DataTransferModels.Session session)
        {
            _logger.LogInformation("GetAllElementValues for session");

            var existingSession = await _sessionRepository.Get(session.Id);
            if (existingSession == null)
            {
                var message = $"Session with id {session.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            Session dataAccessSessionModel = _mapper.Map<Session>(session);
            var dataAccessElementValueModel = await _elementValueRepository.List(dataAccessSessionModel);

            return _mapper.Map<DataTransferModels.ElementValue[]>(dataAccessElementValueModel);
        }

        public async Task<DataTransferModels.ElementValue[]> GetAllElementValues(DataTransferModels.Element element)
        {
            _logger.LogInformation("GetAllElementValues for element");

            var existingElement = await _elementRepository.Get(element.Id);
            if (existingElement == null)
            {
                var message = $"Element with id {element.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            Element dataAccessElementModel = _mapper.Map<Element>(element);
            var dataAccessElementValueModel = await _elementValueRepository.List(dataAccessElementModel);

            return _mapper.Map<DataTransferModels.ElementValue[]>(dataAccessElementValueModel);
        }

        public async Task<DataTransferModels.Form[]> GetAllForms()
        {
            _logger.LogInformation("GetAllForms");

            var dataAccessFormModel = await _formRepository.List();

            return _mapper.Map<DataTransferModels.Form[]>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Form[]> GetAllForms(DataTransferModels.Project project)
        {
            _logger.LogInformation("GetAllForms for project");

            var existingProject = await _projectRepository.Get(project.Id);
            if (existingProject == null)
            {
                var message = $"Project with id {project.Id} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            //Project dataAccessProjectModel = _mapper.Map<Project>(project);
            var dataAccessFormModel = await _formRepository.List(/*dataAccessProjectModel*/existingProject);

            return _mapper.Map<DataTransferModels.Form[]>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Project[]> GetAllProjects(DataTransferModels.User user)
        {
            _logger.LogInformation("GetAllProjects for user");

            var existingUser = await _userRepository.Get(user.GoogleId);
            if (existingUser == null)
            {
                var message = $"User with Google ID {user.GoogleId} does not exist.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            User dataAccessUserModel = _mapper.Map<User>(user);
            var dataAccessProjectModel = await _projectRepository.List(dataAccessUserModel);

            return _mapper.Map<DataTransferModels.Project[]>(dataAccessProjectModel);
        }

        public async Task<DataTransferModels.Session[]> GetAllSessions()
        {
            _logger.LogInformation("GetAllSessions");

            var dataAccessSessionModel = await _sessionRepository.List();
            return _mapper.Map<DataTransferModels.Session[]>(dataAccessSessionModel);
        }

        public async Task<DataTransferModels.User[]> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers");

            var dataAccessUserModel = await _userRepository.List();
            return _mapper.Map<DataTransferModels.User[]>(dataAccessUserModel);
        }

        public async Task<DataTransferModels.Element> GetElement(Guid id)
        {
            _logger.LogInformation("GetElement");
            
            var dataAccessElementModel = await _elementRepository.Get(id);

            return _mapper.Map<DataTransferModels.Element>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.ElementValue> GetElementValue(Guid id)
        {
            _logger.LogInformation("GetElementValue");

            var dataAccessElementValueModel = await _elementValueRepository.Get(id);

            return _mapper.Map<DataTransferModels.ElementValue>(dataAccessElementValueModel);
        }

        public async Task<DataTransferModels.Form> GetForm(Guid id)
        {
            _logger.LogInformation("GetForm");

            var dataAccessFormModel = await _formRepository.Get(id);

            return _mapper.Map<DataTransferModels.Form>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Project> GetProject(Guid id)
        {
            _logger.LogInformation("GetProject");

            var dataAccessProjectModel = await _projectRepository.Get(id);

            return _mapper.Map<DataTransferModels.Project>(dataAccessProjectModel);
        }

        public async Task<DataTransferModels.Session> GetSession(Guid id)
        {
            _logger.LogInformation("GetSession");

            Session dataAccessSessionModel = await _sessionRepository.Get(id);

            return _mapper.Map<DataTransferModels.Session>(dataAccessSessionModel);
        }

        public async Task<DataTransferModels.User> GetUser(Guid id)
        {
            _logger.LogInformation("GetUser");

            User dataAccessUserModel = await _userRepository.Get(id);

            return _mapper.Map<DataTransferModels.User>(dataAccessUserModel);
        }

        public async Task<DataTransferModels.User> GetUser(string googleId)
        {
            _logger.LogInformation("GetUser with Google ID");

            User dataAccessUserModel = await _userRepository.Get(googleId);

            return _mapper.Map<DataTransferModels.User>(dataAccessUserModel);
        }

        public async Task<DataTransferModels.Element> RemoveElement(DataTransferModels.Element element)
        {
            _logger.LogInformation("RemoveElement");

            Element dataAccessElementModel = _mapper.Map<Element>(element);
            dataAccessElementModel = await _elementRepository.Delete(dataAccessElementModel);

            return _mapper.Map<DataTransferModels.Element>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.ElementValue> RemoveElementValue(DataTransferModels.ElementValue elementValue)
        {
            _logger.LogInformation("RemoveElementValue");

            ElementValue dataAccessElementValueModel = _mapper.Map<ElementValue>(elementValue);
            dataAccessElementValueModel = await _elementValueRepository.Delete(dataAccessElementValueModel);

            return _mapper.Map<DataTransferModels.ElementValue>(dataAccessElementValueModel);
        }

        public async Task<DataTransferModels.Form> RemoveForm(DataTransferModels.Form form)
        {
            _logger.LogInformation("RemoveForm");

            Form dataAccessFormModel = _mapper.Map<Form>(form);
            dataAccessFormModel = await _formRepository.Delete(dataAccessFormModel);

            return _mapper.Map<DataTransferModels.Form>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Project> RemoveProject(DataTransferModels.Project project)
        {
            _logger.LogInformation("RemoveProject");

            Project dataAccessProjectModel = _mapper.Map<Project>(project);
            dataAccessProjectModel = await _projectRepository.Delete(dataAccessProjectModel);

            return _mapper.Map<DataTransferModels.Project>(dataAccessProjectModel);
        }

        public async Task<DataTransferModels.Session> RemoveSession(DataTransferModels.Session session)
        {
            _logger.LogInformation("RemoveSession");

            Session dataAccessSessionModel = _mapper.Map<Session>(session);
            dataAccessSessionModel = await _sessionRepository.Delete(dataAccessSessionModel);

            return _mapper.Map<DataTransferModels.Session>(dataAccessSessionModel);
        }

        public async Task<DataTransferModels.User> RemoveUser(DataTransferModels.User user)
        {
            _logger.LogInformation("RemoveUser");

            User dataAccessUserModel = _mapper.Map<User>(user);
            dataAccessUserModel = await _userRepository.Delete(dataAccessUserModel);

            return _mapper.Map<DataTransferModels.User>(dataAccessUserModel);
        }

        public async Task<DataTransferModels.Element> UpdateElement(DataTransferModels.Element element)
        {
            _logger.LogInformation("UpdateElement");

            Element dataAccessElementModel = _mapper.Map<Element>(element);
            dataAccessElementModel = await _elementRepository.Update(dataAccessElementModel);

            return _mapper.Map<DataTransferModels.Element>(dataAccessElementModel);
        }

        public async Task<DataTransferModels.ElementValue> UpdateElementValue(DataTransferModels.ElementValue elementValue)
        {
            _logger.LogInformation("UpdateElementValue");

            ElementValue dataAccessElementValueModel = _mapper.Map<ElementValue>(elementValue);
            dataAccessElementValueModel = await _elementValueRepository.Update(dataAccessElementValueModel);

            return _mapper.Map<DataTransferModels.ElementValue>(dataAccessElementValueModel);
        }

        // updating description and display name only
        public async Task<DataTransferModels.Form> UpdateForm(DataTransferModels.Form form)
        {
            _logger.LogInformation("UpdateForm");

            Form dataAccessFormModel = _mapper.Map<Form>(form);
            dataAccessFormModel = await _formRepository.Update(dataAccessFormModel);

            return _mapper.Map<DataTransferModels.Form>(dataAccessFormModel);
        }

        public async Task<DataTransferModels.Project> UpdateProject(DataTransferModels.Project project)
        {
            _logger.LogInformation("UpdateProject");

            Project dataAccessProjectModel = _mapper.Map<Project>(project);
            dataAccessProjectModel = await _projectRepository.Update(dataAccessProjectModel);

            return _mapper.Map<DataTransferModels.Project>(dataAccessProjectModel);
        }

        public async Task<DataTransferModels.Session> UpdateSession(DataTransferModels.Session session)
        {
            _logger.LogInformation("UpdateSession");

            Session dataAccessSessionModel = _mapper.Map<Session>(session);
            dataAccessSessionModel = await _sessionRepository.Update(dataAccessSessionModel);

            return _mapper.Map<DataTransferModels.Session>(dataAccessSessionModel);
        }

        public async Task<DataTransferModels.User> UpdateUser(DataTransferModels.User user)
        {
            _logger.LogInformation("UpdateUser");

            User dataAccessUserModel = _mapper.Map<User>(user);
            dataAccessUserModel = await _userRepository.Update(dataAccessUserModel);

            return _mapper.Map<DataTransferModels.User>(dataAccessUserModel);
        }
    }
}
