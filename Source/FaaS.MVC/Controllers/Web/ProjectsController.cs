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
        private readonly IProjectService projectService;
        private readonly IFormService formService;
        private readonly IUserService userService;
        private IMapper mapper;

        public ProjectsController(IProjectService projectService, IFormService formService, IUserService userService, IMapper mapper)
        {
            this.projectService = projectService;
            this.formService = formService;
            this.userService = userService;
            this.mapper = mapper;
        }

        // GET: Projects
        [ActionName("Index")]
        public async Task<IActionResult> Index(string id)
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await userService.Get(new Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await projectService.GetAllForUser(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                var projectDTO = await projectService.Get(new Guid(id));
                ViewData["projectName"] = projectDTO.ProjectName;

                HttpContext.Session.SetString("projectId", id);

                var formsDTO = await formService.GetAllForProject(projectDTO);
                ViewBag.FormDictionary = formsDTO.ToDictionary(
                    f => f.Id.ToString(),
                    f => f.FormName.ToString());

                return View(mapper.Map<ProjectViewModel>(projectDTO));
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
                var userDTO = await userService.Get(new Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await projectService.GetAllForUser(userDTO);
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
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await projectService.GetAllForUser(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectDTO = await projectService.Get(new Guid(id));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            return View(mapper.Map<ProjectDetailsViewModel>(projectDTO));
        }

        // GET: Projects/Edit/Id
        public async Task<ActionResult> Edit(string id)
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await userService.Get(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            var projectsDTO = await projectService.GetAllForUser(userDTO);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectDTO = await projectService.Get(new Guid(id));
            if (projectDTO == null)
            {
                RedirectToAction("Index", "Home");
            }
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            HttpContext.Session.SetString("projectToEdit", id);
            return View(mapper.Map<ProjectViewModel>(projectDTO));
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
            var userDTO = await userService.Get(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            try
            {
                var projectDTO = mapper.Map<ProjectViewModel, Project>(model);
                var updatedProject = await projectService.Update(projectDTO);

                return RedirectToAction("Index", "Projects", new { id = projectId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Projects/Delete
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await projectService.GetAllForUser(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var existingProject = await projectService.Get(new Guid(id));
            ViewData["projectName"] = existingProject.ProjectName;

            var formsDTO = await formService.GetAllForProject(existingProject);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            HttpContext.Session.SetString("projectToDelete", id);
            return View(mapper.Map<ProjectViewModel>(existingProject));
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
                var projectDTO = mapper.Map<CreateProjectViewModel, Project>(project);
                string userId = HttpContext.Session.GetString("userId");
                var userDTO = await userService.Get(new Guid(userId));
                var addedProject = await projectService.Add(userDTO, projectDTO);

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
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userDisplayName"] = existingUser.UserName;

            var existingProject = await projectService.Get(new Guid(projectId));
            var deletedProject = await projectService.Remove(existingProject);

            return RedirectToAction("Index", "Home");
        }
    }
}
