using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        // GET: Projects
        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

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

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

                return View(newProject);
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View(newProject);
            }
        }

        // GET: Project/Details
        public async Task<IActionResult> Details(string id)
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

            var existingProject = await _faaSService.GetProject(id);

            return View(_mapper.Map<ProjectDetailsViewModel>(existingProject));
        }

        // GET: Projects/Delete
        public async Task<IActionResult> Delete(string id)
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

            var existingProject = await _faaSService.GetProject(id);
            HttpContext.Session.SetString("projectToDelete", id);
            return View(_mapper.Map<ProjectViewModel>(existingProject));
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

        // POST: Projects/Delete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var projectCodeName = HttpContext.Session.GetString("projectToDelete");

            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            var existingProject = await _faaSService.GetProject(projectCodeName);
            var deletedProject = await _faaSService.RemoveProject(existingProject);

            return RedirectToAction("Index");
        }
    }
}
