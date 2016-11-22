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
        /// Project service
        /// </summary>
        private readonly IProjectService projectService;

        private readonly ILogger<ProjectsController> logger;

        public FormsController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            IFormService formService,
            IProjectService projectService,
            ILogger<ProjectsController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.formService = formService;
            this.projectService = projectService;
            this.logger = logger;
        }

        // GET forms
        [HttpGet]
        public async Task<IActionResult> GetAllForms(
            [FromQuery(Name = "projectId")] Guid projectId,
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var userId = HttpContext.Session.GetString("userId");

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
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var form = await formService.Get(id);
            if (form == null)
            {
                return NotFound("Cannot find form with guid: " + id);
            }
            HttpContext.Session.SetString("formId", form.Id.ToString());

            return Ok(form);
        }

        // POST forms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFormViewModel form)
        {
            try
            {
                form.Created = DateTime.Now;
                var formDto = mapper.Map<CreateFormViewModel, Form>(form);
                var projectId = form.SelectedProjectId;
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
        public async Task<IActionResult> Put([FromBody] FormViewModel form)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var formDto = mapper.Map<FormViewModel, Form>(form);
                var result = await formService.Update(formDto);
                logger.LogInformation("[UPDATE] form: {} ", formDto);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE forms/{id}
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

                var form = await formService.Get(id);
                var result = await formService.Remove(form);
                logger.LogInformation("[DELETE] form: {} ", form);
  

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
