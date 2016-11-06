using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

        // GET: Projects
        [ActionName("Index")]
        public async Task<IActionResult> Index(string id)
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await _faaSService.GetUser(new Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                var projectDTO = await _faaSService.GetProject(new Guid(id));
                ViewData["projectName"] = projectDTO.ProjectName;

                HttpContext.Session.SetString("projectId", id);

                var formsDTO = await _faaSService.GetAllForms(projectDTO);
                ViewBag.FormDictionary = formsDTO.ToDictionary(
                    f => f.Id.ToString(),
                    f => f.FormName.ToString());

                return View(_mapper.Map<ProjectViewModel>(projectDTO));
            }
            else
            {
                ViewData["userName"] = null;
                return View();
            }
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            var newProject = new CreateProjectViewModel();

            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await _faaSService.GetUser(new Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                return View(newProject);
            }
            else
            {
                ViewData["userName"] = null;
                return View(newProject);
            }
        }

        // GET: Project/Details
        public async Task<IActionResult> Details(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await _faaSService.GetUser(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectDTO = await _faaSService.GetProject(new Guid(id));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            return View(_mapper.Map<ProjectDetailsViewModel>(projectDTO));
        }

        // GET: Projects/Edit/Id
        public async Task<ActionResult> Edit(string id)
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(userDTO);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectDTO = await _faaSService.GetProject(new Guid(id));
            if (projectDTO == null)
            {
                RedirectToAction("Index", "Home");
            }
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            HttpContext.Session.SetString("projectToEdit", id);
            return View(_mapper.Map<ProjectViewModel>(projectDTO));
        }

        // POST: Projects/Edit/Id
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectViewModel model)
        {
            var projectId = HttpContext.Session.GetString("projectToEdit");

            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            try
            {
                var projectDTO = _mapper.Map<ProjectViewModel, Project>(model);
                var updatedProject = await _faaSService.UpdateProject(projectDTO);

                return RedirectToAction("Index", "Projects", new { id = projectId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Projects/Delete
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await _faaSService.GetUser(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var existingProject = await _faaSService.GetProject(new Guid(id));
            ViewData["projectName"] = existingProject.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(existingProject);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            HttpContext.Session.SetString("projectToDelete", id);
            return View(_mapper.Map<ProjectViewModel>(existingProject));
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectName", "Description", "Created")]CreateProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                var projectDTO = _mapper.Map<CreateProjectViewModel, Project>(project);
                string userId = HttpContext.Session.GetString("userId");
                var userDTO = await _faaSService.GetUser(new Guid(userId));
                var addedProject = await _faaSService.AddProject(userDTO, projectDTO);

                return RedirectToAction("Index", "Home");
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
            var projectId = HttpContext.Session.GetString("projectToDelete");

            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await _faaSService.GetUser(new Guid(userId));

            ViewData["userDisplayName"] = existingUser.UserName;

            var existingProject = await _faaSService.GetProject(new Guid(projectId));
            var deletedProject = await _faaSService.RemoveProject(existingProject);

            return RedirectToAction("Index", "Home");
        }
    }
}
