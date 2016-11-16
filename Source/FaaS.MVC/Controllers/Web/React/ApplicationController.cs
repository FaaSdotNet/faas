using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaaS.MVC.Controllers.Web.React
{
    [Route("")]
    public class ApplicationController : Controller
    {

        public async Task<IActionResult> Index()
        {

            return View("~/Views/React/Application.cshtml");
        }
    }
}
