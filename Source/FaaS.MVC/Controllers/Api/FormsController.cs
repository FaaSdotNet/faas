using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.DataTransferModels;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class FormsController : DefaultController
    {
        public const string RouteController = "forms";

        /// <summary>
        /// Form service
        /// </summary>
        private readonly IFormService formService;

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Project service
        /// </summary>
        private readonly IProjectService projectService;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ProjectsController> logger;

        public FormsController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            IFormService formService,
            IUserService userService,
            IProjectService projectService,
            ILogger<ProjectsController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.formService = formService;
            this.userService = userService;
            this.projectService = projectService;
            this.logger = logger;
        }

        // GET forms
        [HttpGet]
        public async Task<IActionResult> GetAllForms(
            [FromQuery(Name = "projectId")] Guid projectId,
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes,
            [FromQuery(Name = "userId")]string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var projectDto = await projectService.Get(projectId);
            if (projectDto == null)
            {
                return NotFound("Project not found with guid:" + projectId);
            }

            var forms = await formService.GetAllForProject(projectDto);

            // Apply limit
            if (limit > 0)
            {
                forms = forms.Take(limit).ToArray();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                forms = forms.Select(user =>
                {
                    var projection = new Form();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, user.GetType().GetProperty(attribute).GetValue(user));
                    }

                    return projection;
                }).ToArray();
            }

            logger.LogInformation($"Retrieved {forms.Length} forms.");
            return Ok(forms);
        }

        // GET form/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(Guid id)
        {
            var form = await formService.Get(id);
            if (form == null)
            {
                return NotFound("Cannot find form with guid: " + id);
            }

            return Ok(form);
        }

        // POST forms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FormViewModel form)
        {
            try
            {
                form.Created = DateTime.Now;
                var formDto = mapper.Map<FormViewModel, Form>(form);
                var projectId = form.ProjectId;
                var projectDto = await projectService.Get(projectId);
                var result = await formService.Add(projectDto, formDto);

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetForm", "Forms", new
                {
                    id = formDto.Id,
                }, httpContextAccessor.HttpContext.Request.Scheme));
                logger.LogInformation("[CREATE] form: {} ", formDto);

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
        public async Task<IActionResult> Put([FromBody] FormViewModel form, [FromQuery(Name = "userId")] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var formDto = mapper.Map<FormViewModel, Form>(form);

                // Access validation
                var formOwner = await userService.Get(new Guid(userId));
                var formToBeUpdated = await formService.Get(formDto.Id);
                if (formToBeUpdated.Project.User.Email == formOwner.Email)
                {
                    var result = await formService.Update(formDto);
                    logger.LogInformation("[UPDATE] form: {} ", formDto);
                    return Ok(result);
                }
                else
                {
                    Response.StatusCode = 401;
                    return BadRequest("You tried to update the form that does not belong to you");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE forms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery(Name = "userId")] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                // Access validation
                var formOwner = await userService.Get(new Guid(userId));
                var formToBeDeleted = await formService.Get(id);
                if (formToBeDeleted.Project.User.Email == formOwner.Email)
                {
                    var result = await formService.Remove(formToBeDeleted);
                    logger.LogInformation("[DELETE] form: {} ", formToBeDeleted);
                    return Ok(result);
                }
                else
                {
                    Response.StatusCode = 401;
                    return BadRequest("You tried to delete a form that does not belong to you");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
