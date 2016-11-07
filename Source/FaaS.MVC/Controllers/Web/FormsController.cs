using AutoMapper;
using FaaS.DataTransferModels;
using FaaS.MVC.Models;
using FaaS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web
{
    public class FormsController : Controller
    {
        private readonly IFormService formService;
        private readonly IElementService elementService;
        private readonly IProjectService projectService;
        private readonly IUserService userService;
        private IMapper mapper;

        public FormsController(IFormService formService, IElementService elementService, IProjectService projectService, IUserService userService, IMapper mapper)
        {
            this.formService = formService;
            this.elementService = elementService;
            this.projectService = projectService;
            this.userService = userService;
            this.mapper = mapper;
        }

        // GET: Forms/Index/id
        public async Task<ActionResult> Index(string id)
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

            string projectId = HttpContext.Session.GetString("projectId");

            var projectDTO = await projectService.Get(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            var formDTO = await formService.Get(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

            HttpContext.Session.SetString("formId", id);

            var elementsDTO = await elementService.GetAllForForm(formDTO);
            ViewBag.ElementDictionary = elementsDTO.ToDictionary(
                e => e.Id.ToString(),
                e => e.Description.ToString());

            return View(mapper.Map<FormViewModel>(formDTO));
        }

        // GET: Forms/Details/Id
        public async Task<ActionResult> Details(string id)
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

            string projectId = HttpContext.Session.GetString("projectId");

            var projectDTO = await projectService.Get(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            var formDTO = await formService.Get(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

            return View(mapper.Map<FormDetailsViewModel>(formDTO));
        }

        // GET: Forms/Create
        public async Task<ActionResult> Create()
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }

            var userDTO = await userService.Get(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;
            SelectList projects = new SelectList(await projectService.GetAllForUser(userDTO), "Id", "ProjectName");

            CreateFormViewModel model = new CreateFormViewModel
            {
                ProjectList = projects
            };

            return View(model);
        }

        // POST: Forms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var allForms = await formService.GetAll();
                    var formDTO = mapper.Map<CreateFormViewModel, Form>(form);

                    formDTO.Created = DateTime.Now;

                    var projectDTO = await projectService.Get(form.SelectedProjectId);
                    await formService.Add(projectDTO, formDTO);
                    return RedirectToAction("Index", "Projects", new { id = form.SelectedProjectId.ToString() });
                }
                catch
                {
                    return RedirectToAction("Index", "Home");   // error page or something
                }
            }
            return View(form);
        }

        // GET: Forms/Edit/5
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

            string projectId = HttpContext.Session.GetString("projectId");

            var projectDTO = await projectService.Get(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            var existingForm = await formService.Get(new Guid(id));
            if (existingForm == null)
            {
                RedirectToAction("Index", "Projects");
            }
            ViewData["formName"] = existingForm.FormName;

            HttpContext.Session.SetString("formToEdit", id);
            return View(mapper.Map<FormViewModel>(existingForm));
        }

        // POST: Forms/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormViewModel model)
        {
            var formId = HttpContext.Session.GetString("formToEdit");

            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await userService.Get(new Guid(userId));
            ViewData["userDisplayName"] = userDTO.UserName;

            try
            {
                var formDTO = mapper.Map<FormViewModel, Form>(model);
                var updatedForm = await formService.Update(formDTO);

                return RedirectToAction("Index", "Forms", new { id = updatedForm.Id });
            }
            catch(Exception)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Forms/Delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await projectService.GetAllForUser(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectId = HttpContext.Session.GetString("projectId");
            var projectDTO = await projectService.Get(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            var formDTO = await formService.Get(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

            HttpContext.Session.SetString("formToDelete", id);
            return View(mapper.Map<FormViewModel>(formDTO));
        }

        // POST: Forms/Delete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var formId = HttpContext.Session.GetString("formToDelete");

            var projectId = HttpContext.Session.GetString("projectId");

            var existingForm = await formService.Get(new Guid(formId));
            var deletedForm = await formService.Remove(existingForm);

            return RedirectToAction("Index", "Projects", new { id = projectId});
        }
    }
}