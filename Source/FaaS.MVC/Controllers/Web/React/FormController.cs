﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaaS.MVC.Controllers.Web.React
{
    public class FormController : Controller
    {
        private readonly IFaaSService faaSService;
        private IMapper _mapper;

        public FormController(IFaaSService faaSService, IMapper mapper)
        {
            this.faaSService = faaSService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View("~/Views/React/Form.cshtml");
        }
    }
}
