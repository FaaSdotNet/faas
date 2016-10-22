using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.MVC.Models.Forms;
using FaaS.MVC.Models.Projects;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Mvc;

namespace FaaS.MVC.Controllers.Web
{
    public class ProjectsController: Controller
    {
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;
        private static User _superUser = new User();

        public ProjectsController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: Products
        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            var projectsDTO = await _faaSService.GetAllProjects(_superUser);
            return View(_mapper.Map<IEnumerable<ProjectModelView>>(projectsDTO));
        }
    }
}
