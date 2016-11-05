using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix+RouteController)]
    public class ProjectsContoller: Controller
    {
        public const string RoutePrefix = "api/v1.0/";
        public const string RouteController = "projects";

        private readonly IFaaSService service;
        private readonly IRandomIdService randomId;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly ILogger<ProjectsContoller> logger;
        private IMapper mapper;
        public ProjectsContoller(IFaaSService service, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<ProjectsContoller> logger, IMapper mapper)
        {
            this.service = service;
            this.randomId = randomId;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelperFactory = urlHelperFactory;
            this.logger = logger;
            this.mapper = mapper;
        }



        // GET projects

        [HttpGet]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var userDto = await service.GetUser(new Guid(userId));
           
            var projects = await service.GetAllProjects(userDto);
            

            // Apply limit
            if (limit > 0)
            {
                projects = projects.Take(limit).ToArray();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                projects = projects.Select(user =>
                {
                    var projection = new Project();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, user.GetType().GetProperty(attribute).GetValue(user));
                    }

                    return projection;
                }).ToArray();
            }

            logger.LogInformation($"Retrieved {projects.Length} projects.");
            return Ok(projects);
        }



        // GET project/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var project = await service.GetProject(id);

            if(project == null)
            {
                return NotFound("Cannot find any project");
            }

            return Ok(project);
        }

        // POST projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateProjectViewModel project)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }
                var userDto = await service.GetUser(userId);

                logger.LogInformation("Creating new project: {} with \"{}\" ", project.ProjectName, project.Description);

                var projectDto = mapper.Map<CreateProjectViewModel, Project>(project);
                if (projectDto == null)
                {
                    logger.LogError("Mapping problem!!!!");
                }

                var result = await service.AddProject(userDto, projectDto);

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetProject", "Projects", new
                {
                    id = project.Id,
                }, httpContextAccessor.HttpContext.Request.Scheme));
                logger.LogInformation("Generated new project with name " + projectDto.ProjectName);

                return Created(newUrl, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT 
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProjectViewModel project)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var projectDto = mapper.Map<ProjectViewModel, Project>(project);
                var result = await service.UpdateProject(projectDto);
              
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

                var project = await service.GetProject(id);
                var result = await service.RemoveProject(project);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
