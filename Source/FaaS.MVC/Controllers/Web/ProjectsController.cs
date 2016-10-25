using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web
{
    public class ProjectsController: Controller
    {
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;

        public ProjectsController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: Products
        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewData["userDisplayName"] = userDTO.DisplayName;
                return View(_mapper.Map<IEnumerable<ProjectViewModel>>(projectsDTO));
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View();
            }
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            var newProject = new CreateProjectViewModel();

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;
                return View(newProject);
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View(newProject);
            }
        }

        public async Task<IActionResult> Details()
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            return View(_mapper.Map<UserDetailsViewModel>(existingUser));
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectCodeName", "DisplayName", "Description", "Created")]CreateProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                var projectDTO = _mapper.Map<CreateProjectViewModel, Project>(project);
                string userCodeName = HttpContext.Session.GetString("userCodeName");
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                var addedProject = await _faaSService.AddProject(userDTO, projectDTO);

                return RedirectToAction("Index");
            }
            return View(project);
        }
    }
}
