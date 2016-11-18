using System;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FaaS.DataTransferModels;
using FaaS.MVC.Models.React;
using System.Collections.Generic;

namespace FaaS.MVC.Controllers.Web.React
{
    public class FormController : Controller
    {
        private readonly IFormService formService;
        private readonly IElementService elementService;
        private IMapper _mapper;

        public FormController(IFormService formService, IElementService elementService, IMapper mapper)
        {
            this.elementService = elementService;
            this.formService = formService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string formId)
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

            FillFormViewModel model = new FillFormViewModel // TODO use this instead of collection of elements (no form name in collection)
            {
                Form = form,
                Elements = elements,
            };

            return View("~/Views/React/Form.cshtml",(object)JsonConvert.SerializeObject(model));
        }
    }
}
