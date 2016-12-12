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
using NuGet.Protocol.Core.v3;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class ProjectsController: DefaultController
    {
        public const string RouteController = "projects";

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Project service
        /// </summary>
        private readonly IProjectService projectService;

        private readonly ILogger<ProjectsController> logger;

        public ProjectsController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            IUserService userService,
            IProjectService projectService,
            ILogger<ProjectsController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.logger = logger;
        }

        // GET projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes,
            [FromQuery(Name = "userId")]string userId)
        {
            logger.LogInformation("User id: {}", userId);
            logger.LogInformation("User id: {}", HttpContext.Session.ToJson());

            if (String.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var userDto = await userService.Get(new Guid(userId));
           
            var projects = await projectService.GetAllForUser(userDto);

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

        // GET project/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id, [FromQuery(Name ="userId")]string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var project = await projectService.Get(id);

            if (project == null)
            {
                return NotFound("Cannot find project with id: " + id.ToString());
            }

            return Ok(project);
        }

        // POST projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectViewModel project, [FromQuery(Name = "userId")] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }
                
                var userDto = await userService.Get(new Guid(userId));
                if(userDto == null)
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                project.Created = DateTime.Now;
                var projectDto = mapper.Map<ProjectViewModel, Project>(project);
                if (projectDto == null)
                {
                    logger.LogError("Mapping problem!!!!");
                    return BadRequest();
                }

                logger.LogInformation("[CREATE] project: {} ", projectDto);


                var result = await projectService.Add(userDto, projectDto);

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
        [HttpPatch]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProjectViewModel project, [FromQuery(Name="userId")] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var projectDto = mapper.Map<ProjectViewModel, Project>(project);

                // Access validation
                var projectOwner = await userService.Get(new Guid(userId));
                var projectToBeUpdated = await projectService.Get(projectDto.Id);
                if (projectToBeUpdated.User.Email == projectOwner.Email)
                {
                    var result = await projectService.Update(projectDto);
                    logger.LogInformation("[UPDATE] Project: " + project);
                    return Ok(result);
                }
                else
                {
                    Response.StatusCode = 401;
                    return BadRequest("You tried to update the project that does not belong to you");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE projects/{id}
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
                var projectOwner = await userService.Get(new Guid(userId));
                var projectToBeDeleted = await projectService.Get(id);
                if (projectToBeDeleted.User.Email == projectOwner.Email)
                {
                    var result = await projectService.Remove(projectToBeDeleted);
                    logger.LogInformation("[UPDATE] Project: " + projectToBeDeleted);
                    return Ok(result);
                }
                else
                {
                    Response.StatusCode = 401;
                    return BadRequest("You tried to delete the project that does not belong to you");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
