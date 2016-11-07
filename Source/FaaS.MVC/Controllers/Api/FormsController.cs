using System;
using System.Collections.Generic;
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
    public class FormsController : DefaultController
    {
        public const string RouteController = "forms";

        private readonly IFaaSService service;
        private readonly ILogger<ProjectsContoller> logger;

        public FormsController(IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IMapper mapper, IFaaSService service, ILogger<ProjectsContoller> logger) : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.service = service;
            this.logger = logger;
        }


        // GET users
        [HttpGet]
        public async Task<IActionResult> GetAllForms(
            [FromQuery(Name = "project")] Guid projectGuid,
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }


            var projectDto = await service.GetProject(projectGuid);

            if (projectDto == null)
            {
                return NotFound("Project not found with guid:" + projectGuid);
            }

            var forms = await service.GetAllForms(projectDto);


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

        // GET project/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(Guid id)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var form = await service.GetForm(id);

            if (form == null)
            {
                return NotFound("Cannot find form with guid: " + id);
            }

            return Ok(form);
        }


        // POST projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateFormViewModel form)
        {
            try
            {
                form.Created = DateTime.Now;
                var formDto = mapper.Map<CreateFormViewModel, Form>(form);
                var projectId = form.SelectedProjectId;
                var projectDto = await service.GetProject(projectId);
                var result = await service.AddForm(projectDto, formDto);

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetForm", "Forms", new
                {
                    id = formDto.Id,
                }, httpContextAccessor.HttpContext.Request.Scheme));
                logger.LogInformation("Generated new project with name " + formDto.FormName);

                return Created(newUrl, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT 
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
                var result = await service.UpdateForm(formDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE projects/{id}
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

                var form = await service.GetForm(id);
                var result = await service.RemoveForm(form);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
