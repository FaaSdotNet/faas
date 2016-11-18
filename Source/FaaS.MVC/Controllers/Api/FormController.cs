using Microsoft.AspNetCore.Mvc;
using FaaS.Services;
using FaaS.Services.Interfaces;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using FaaS.MVC.Models.React;
using FaaS.DataTransferModels;
using System.Linq;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class FormController : DefaultController
    {

        public const string RouteController = "form";

        private readonly IFormService formService;
        private readonly IElementService elementService;
        private readonly IElementValueService elementValueService;
        private readonly ISessionService sessionService;

        private readonly ILogger<FormController> logger;

        public FormController(IRandomIdService randomId,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IMapper mapper,
            IFormService formService,
            IElementService elementService,
            IElementValueService elementValueService,
            ISessionService sessionService,
            ILogger<FormController> logger)
            : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.formService = formService;
            this.elementService = elementService;
            this.elementValueService = elementValueService;
            this.sessionService = sessionService;
            this.logger = logger;
        }

        // POST forms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FillFormViewModel model)
        {
            try
            {
                Form existingForm = await formService.Get(model.Form.Id);
                List<Element> existingElements = new List<Element> (await elementService.GetAllForForm(existingForm));

                if (model.Elements.Length != existingElements.Count() || model.Values.Length != existingElements.Count())
                {
                    return BadRequest("Wrong data");
                }

                for (int i = 0; i < model.Elements.Length; i++)
                {
                    if((model.Elements[i].Required && model.Values[i] == null) ||
                        !existingElements.Select(e => e.Id).Contains(model.Elements[i].Id))
                    {
                        return BadRequest("Wrong data");
                    }
                }

                Session session = new Session
                {
                    Filled = DateTime.Now
                };

                session = await sessionService.Add(session);
                if(session == null)
                {
                    return BadRequest("Problem on server side"); // TODO
                }

                for (int i = 0; i < model.Elements.Length;i++)
                {
                    ElementValue newElementValue = new ElementValue
                    {
                        Element = model.Elements[i],
                        Value = model.Values[i]
                    };

                    existingElements[i].Form = existingForm;    //workarround

                    var added = await elementValueService.Add(existingElements[i], session, newElementValue);
                    if(added == null)
                    {
                        return BadRequest("Problem on server side"); // TODO
                    }
                }

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("Thankyou", "Form", new {}, httpContextAccessor.HttpContext.Request.Scheme));

                logger.LogInformation($"Added {existingElements.Count} values of form {existingForm.Id}: {existingForm.Description}");

                return Created(newUrl, null);
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

public class Whatever
{
    public string Guid { get; set; }
    public string jsonString { get; set; }
}

public class ListOfWhatevers
{
    public List<Whatever> Whatevers { get; set; }
}

