using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FaaS.MVC.Controllers.Web.React
{
    public class LoginController : Controller
    {
        private readonly IFaaSService faaSService;
        private IMapper _mapper;

        public LoginController(IFaaSService faaSService, IMapper mapper)
        {
            this.faaSService = faaSService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View("~/Views/React/Login.cshtml");
        }
    }
}
