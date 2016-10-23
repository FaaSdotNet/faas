using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Mvc;

namespace FaaS.MVC.Controllers.Web
{
    public class FromsController : Controller
    {
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;
        private static Project superProject = new Project();

        public FromsController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: Products
        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            var formsDTO = await _faaSService.GetAllForms(superProject);
            return View(_mapper.Map<IEnumerable<ProjectViewModel>>(formsDTO));
        }
    }
}