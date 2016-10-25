using System;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/[controller]")]
    //[Route("api/projects")]
    public class ProjectController : Controller
    {
        private readonly IFaaSService _faaSService;
        private readonly IRandomIdService _randomId;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly ILogger<ProjectController> _logger;

        private static readonly User _superUser = new User();

        public ProjectController(IFaaSService gameStockService, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<ProjectController> logger)
        {
            _faaSService = gameStockService;
            _randomId = randomId;
            _actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _logger = logger;
        }

        // GET api/projects
        [HttpGet]
        //public async Task<IActionResult> GetAllGames(
        public async Task<IActionResult> GetAllProjects(
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            // TODO
            var projects = await _faaSService.GetAllProjects(_superUser);

           // Apply limit
            if (limit > 0)
            {
                projects = projects.Take(limit).ToArray();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                projects = projects.Select(project =>
                {
                    var projection = new FaaS.Services.DataTransferModels.Project();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, project.GetType().GetProperty(attribute).GetValue(project));
                    }

                    return projection;
                }).ToArray();
            }

            _logger.LogInformation($"Retrieved {projects.Length} projects.");
            return Ok(projects);
        }

        // GET api/projects/wow
        [HttpGet("{codename}")]
        public async Task<IActionResult> GetGame(string codename)
        {
            var game = await _faaSService.GetProject(codename);
            return Ok(game);
        }

        // POST api/projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Project project)
        {
            try
            {
                var result = await _faaSService.AddProject(_superUser, project);

                var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetProject", "Projects", new
                {
                    codename = project.ProjectCodeName,
                }, _httpContextAccessor.HttpContext.Request.Scheme));
                _logger.LogInformation("Generated new game with name " + project.DisplayName);

                return Created(newUrl, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/projects/wow
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/projects/wow
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
