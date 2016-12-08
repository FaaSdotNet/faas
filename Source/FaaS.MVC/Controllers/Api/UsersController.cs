using AutoMapper;
using FaaS.DataTransferModels;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]

    public class UsersController : DefaultController
    {
        public const string RouteController = "users";

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        private readonly IProjectService projectService;
        private readonly IFormService formService;
        private readonly IElementService elementService;

        private readonly ILogger<UsersController> logger;

        public UsersController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            IUserService userService,
            IProjectService projectService,
            IFormService formService,
            IElementService elementService,
            ILogger<UsersController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.formService = formService;
            this.elementService = elementService;
            this.logger = logger;
        }

        // GET users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var users = await userService.GetAll();

            // Apply limit
            if (limit > 0)
            {
                users = users.Take(limit).ToArray();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                users = users.Select(user =>
                {
                    var projection = new User();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, user.GetType().GetProperty(attribute).GetValue(user));
                    }

                    return projection;
                }).ToArray();
            }

            logger.LogInformation($"Retrieved {users.Length} users.");
            return Ok(users);
        }

        // GET users/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var user = await userService.Get(id);

            if (user == null)
            {
                return NotFound("Cannot find user with id = " + id.ToString());
            }

            return Ok(user);
        }


        // POST projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserViewModel user)
        {
            try
            {
                user.Registered = DateTime.Now;
                var userDto = mapper.Map<UserViewModel, User>(user);
                var result = await userService.Add(userDto);

                GenerateTemplateForm(result);

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetUser", "Users", new
                {
                    id = user.Id,
                }, httpContextAccessor.HttpContext.Request.Scheme));
                logger.LogInformation("Generated new user with name " + userDto.Name);

                return Created(newUrl, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [HttpPatch]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserViewModel user)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var userDto = mapper.Map<UserViewModel, User>(user);
                var result = await userService.Update(userDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var user = await userService.Get(id);
                var result = await userService.Remove(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async void GenerateTemplateForm(User newUser)
        {
            var templateProject = new Project();
            templateProject.ProjectName = "TemplateProject";
            templateProject.Created = DateTime.Now;
            templateProject.Description = "For illustration purposes only.";

            templateProject = await projectService.Add(newUser, templateProject);

            var templateForm = new Form();
            templateForm.FormName = "TemplateForm";
            templateForm.Created = DateTime.Now;
            templateForm.Description = "For illustration purposes only.";

            templateForm = await formService.Add(templateProject, templateForm);

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

            templateElement = await elementService.Add(templateForm, templateElement);
            templateElement2 = await elementService.Add(templateForm, templateElement2);
            templateElement3 = await elementService.Add(templateForm, templateElement3);
        }
    }
}
