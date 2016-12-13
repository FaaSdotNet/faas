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
using FaaS.Services.Interfaces;
using System.Collections.Generic;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class SessionsController : DefaultController
    {
        public const string RouteController = "sessions";

        /// <summary>
        /// Session service
        /// </summary>
        private readonly ISessionService sessionService;

        private readonly ILogger<ProjectsController> logger;

        public SessionsController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            ISessionService sessionService,
            ILogger<ProjectsController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.sessionService = sessionService;
            this.logger = logger;
        }

        // GET sessions
        [HttpGet]
        public async Task<IActionResult> GetAllSessions(
            [FromQuery(Name = "formId")] Guid formId)
        {
            Session[] allSessions = await sessionService.GetAll();

            List<Session> result = new List<Session>();

            foreach (Session session in allSessions)
            {
                ElementValue elementValue = session.ElementValues.ElementAtOrDefault(0);
                if (elementValue == null)
                {
                    continue;
                }
                if (elementValue.Element.Form.Id == formId)
                {
                    result.Add(session);
                }
            }

            logger.LogInformation($"Retrieved {result.Count} sessions.");
            return Ok(result.ToArray().OrderByDescending(e => e.Filled));
        }

        // GET session/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSession(Guid id)
        {
            var session = await sessionService.Get(id);

            if (session == null)
            {
                return NotFound("Cannot find session with id: " + id.ToString());
            }

            return Ok(session);
        }

    }
}
