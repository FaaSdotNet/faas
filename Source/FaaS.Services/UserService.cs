using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FaaS.Entities.Repositories;
using FaaS.DataTransferModels;

namespace FaaS.Services
{
    /// <summary>
    /// Implementation of <see cref="IUserService"/> interface
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IUserService> logger;

        /// <summary>
        /// User repository
        /// </summary>
        private readonly IUserRepository userRepository;

        private readonly IFormRepository formRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IElementRepository elementRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository">user repository</param>
        /// <param name="logger">logger</param>
        public UserService(IUserRepository userRepository,
            IProjectRepository projectRepository,
            IFormRepository formRepository,
            IElementRepository elementRepository,
            ILogger<IUserService> logger)
        {
            this.userRepository = userRepository;
            this.projectRepository = projectRepository;
            this.formRepository = formRepository;
            this.elementRepository = elementRepository;
            this.logger = logger;
        }

        public async Task<User> Add(User user)
        {
            logger.LogInformation("Add operation called");

            var existingUser = await userRepository.Get(user.Email);
            if (existingUser == null)
            {
                // Set registered date and add a new user
                user.Registered = DateTime.Now;
                var newUser = await userRepository.Add(user);
                GenerateTemplateForm(newUser);

                return newUser;
            }
            else
            {
                // Set new token, update existing user
                existingUser.GoogleToken = user.GoogleToken;
                return await userRepository.Update(existingUser);
            }
        }

        public async Task<User> Get(string email)
        {
            logger.LogInformation("Get [email] operation called");

            return await userRepository.Get(email);
        }

        public async Task<User> Get(Guid id)
        {
            logger.LogInformation("Get [guid] operation called");

            return await userRepository.Get(id);
        }

        public async Task<User> GetByToken(string token)
        {
            logger.LogInformation("GetByToken operation called");

            return await userRepository.GetByToken(token);
        }

        public async Task<User[]> GetAll()
        {
            logger.LogInformation("GetAll operation called");

            return (await userRepository.List()).ToArray();
        }

        public async Task<User> Remove(User user)
        {
            logger.LogInformation("Remove operation called");

            return await userRepository.Delete(user);
        }

        public async Task<User> Update(User user)
        {
            logger.LogInformation("Update operation called");

            return await userRepository.Update(user);
        }

        private async void GenerateTemplateForm(User newUser)
        {
            var templateProject = new Project();
            templateProject.ProjectName = "TemplateProject";
            templateProject.Created = DateTime.Now;
            templateProject.Description = "For illustration purposes only.";

            templateProject = await projectRepository.Add(newUser, templateProject);

            var templateForm = new Form();
            templateForm.FormName = "TemplateForm";
            templateForm.Created = DateTime.Now;
            templateForm.Description = "For illustration purposes only.";

            templateForm = await formRepository.Add(templateProject, templateForm);

            var templateElement = new Element();
            templateElement.Description = "Enter your birthday:";
            templateElement.Type = 1;
            templateElement.Required = true;
            templateElement.Options = "";

            var templateElement2 = new Element();
            templateElement2.Description = "Describe how was your day today:";
            templateElement2.Type = 5;
            templateElement2.Required = false;
            templateElement2.Options = "";

            var templateElement3 = new Element();
            templateElement3.Description = "Which one do you prefer?";
            templateElement3.Type = 2;
            templateElement3.Required = true;
            templateElement3.Options = "{\"1\":\"C#\",\"2\":\"JavaScript\"}";

            templateElement = await elementRepository.Add(templateForm, templateElement);
            templateElement2 = await elementRepository.Add(templateForm, templateElement2);
            templateElement3 = await elementRepository.Add(templateForm, templateElement3);
        }
    }
}
