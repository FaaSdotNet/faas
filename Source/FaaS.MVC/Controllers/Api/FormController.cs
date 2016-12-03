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


        //GET form
        [HttpGet]
        public async Task<IActionResult> Get(string formId)
        {
            Form form = await formService.Get(new Guid(formId));
            if (form == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Element[] elements = await elementService.GetAllForForm(form);
            ElementValue[] values = new ElementValue[elements.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new ElementValue();
            }

            FillFormModel model = new FillFormModel // TODO use this instead of collection of elements (no form name in collection)
            {
                Form = form,
                Elements = elements,
            };

            return Ok(model);
        }

        // POST form
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FillFormModel model)
        {
            try
            {
                Form existingForm = await formService.Get(model.Form.Id);
                List<Element> existingElements = new List<Element> (await elementService.GetAllForForm(existingForm));

                if (model.Elements.Length != existingElements.Count() || model.Values.Length != existingElements.Count())
                {
                    return BadRequest("Wrong data");
                }

                if (model.Elements.Where((t, i) => (t.Required && model.Values[i] == null) ||
                                                   !existingElements.Select(e => e.Id).Contains(t.Id)).Any())
                {
                    return BadRequest("Wrong data");
                }

                var session = new Session
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
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  // TODO
            }
        }
    }
}

public class FillFormModel
{
    public Form Form { get; set; }
    public Element[] Elements { get; set; }
    public string[] Values { get; set; }
}