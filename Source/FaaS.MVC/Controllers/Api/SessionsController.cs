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

        private readonly IFormService formService;
        private readonly IElementService elementService;
        private readonly IElementValueService elementValueService;

        private readonly ILogger<ProjectsController> logger;

        public SessionsController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            ISessionService sessionService,
            IFormService formService,
            IElementService elementService,
            IElementValueService elementValueService,
            ILogger<ProjectsController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.sessionService = sessionService;
            this.formService = formService;
            this.elementService = elementService;
            this.elementValueService = elementValueService;
            this.logger = logger;
        }

        // GET sessions
        [HttpGet]
        public async Task<IActionResult> GetAllSessions(
            [FromQuery(Name = "formId")] Guid formId,
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var formDto = await formService.Get(formId);
            if (formDto == null)
            {
                return NotFound("Form not found with guid:" + formId);
            }

            var elements = await elementService.GetAllForForm(formDto);
            HashSet<Guid> uniqueSessions = new HashSet<Guid>();

            foreach (Element element in elements)
            {
                var elementValues = await elementValueService.GetAllForElement(element);

                var uniquePart = elementValues.Select(elementValue => elementValue.Session.Id).Distinct();
                uniqueSessions.UnionWith(uniquePart);
            }

            var sessions = new List<Session>();
            foreach (Guid sessionId in uniqueSessions)
            {
                var session = await sessionService.Get(sessionId);
                sessions.Add(session);
            }

            // Apply limit
            if (limit > 0)
            {
                sessions = sessions.Take(limit).ToList();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                sessions = sessions.Select(session =>
                {
                    var projection = new Session();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, session.GetType().GetProperty(attribute).GetValue(session));
                    }

                    return projection;
                }).ToList();
            }

            logger.LogInformation($"Retrieved {sessions.Count} sessions.");
            return Ok(sessions.ToArray());
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
