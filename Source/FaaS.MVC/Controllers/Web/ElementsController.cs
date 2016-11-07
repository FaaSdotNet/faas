using AutoMapper;
using FaaS.DataTransferModels;
using FaaS.MVC.Models;
using FaaS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web
{
    public class ElementsController : Controller
    {
        private readonly IElementService elementService;
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private readonly IFormService formService;
        private IMapper mapper;

        public ElementsController(IElementService elementService, IUserService userService, IProjectService projectService, IFormService formService, IMapper mapper)
        {
            this.elementService = elementService;
            this.userService = userService;
            this.projectService = projectService;
            this.formService = formService;
            this.mapper = mapper;
        }

        // GET: Elements/Create
        public async Task<IActionResult> Create()
        {
            var newElement = new CreateElementViewModel();

            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
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

                string formId = HttpContext.Session.GetString("formId");
                var formDTO = await formService.Get(new Guid(formId));
                ViewData["formName"] = formDTO.FormName;
                ViewData["formId"] = formDTO.Id;

                return View(newElement);
            }
            else
            {
                ViewData["userName"] = null;
                return View(newElement);
            }
        }

        // GET: Element/Details/Id
        public async Task<IActionResult> Details(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await projectService.GetAllForUser(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            string projectId = HttpContext.Session.GetString("projectId");
            var projectDTO = await projectService.Get(new Guid(projectId));
            ViewData["projectId"] = projectDTO.ProjectName;

            var formsDTO = await formService.GetAllForProject(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            string formId = HttpContext.Session.GetString("formId");
            var formDTO = await formService.Get(new Guid(formId));

            ViewData["formName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var existingElement = await elementService.Get(new Guid(id));

            return View(mapper.Map<ElementDetailsViewModel>(existingElement));
        }

        // POST: Elements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateElementViewModel element)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var allElements = await elementService.GetAll();
                    var elementDTO = mapper.Map<CreateElementViewModel, Element>(element);

                    string formId = HttpContext.Session.GetString("formId");
                    var formDTO = await formService.Get(new Guid(formId));
                    await elementService.Add(formDTO, elementDTO);
                    return RedirectToAction("Index", "Forms", new { id = formId });
                }
                catch
                {
                    return RedirectToAction("Index", "Home");   // error page or something
                }
            }
            return View(element);
        }

        // GET: Elements/Edit/Id
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

            string formId = HttpContext.Session.GetString("formId");
            var formDTO = await formService.Get(new Guid(formId));
            ViewData["formName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var existingElement = await elementService.Get(new Guid(id));
            if (existingElement == null)
            {
                RedirectToAction("Index", "Projects");
            }

            HttpContext.Session.SetString("elementToEdit", id);
            return View(mapper.Map<ElementViewModel>(existingElement));
        }

        // POST: Elements/Edit/Id
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ElementViewModel model)
        {
            var elementId = HttpContext.Session.GetString("elementToEdit");

            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await userService.Get(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            try
            {
                var elementDTO = mapper.Map<ElementViewModel, Element>(model);
                elementDTO.Id = new Guid(elementId);
                var updatedElement = await elementService.Update(elementDTO);

                string formId = HttpContext.Session.GetString("formId");
                var formDTO = await formService.Get(new Guid(formId));

                return RedirectToAction("Index", "Forms", new { id = formId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Elements/Delete/Id
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

            var formId = HttpContext.Session.GetString("formId");
            var formDTO = await formService.Get(new Guid(formId));
            ViewData["formDisplayName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var elementDTO = await elementService.Get(new Guid(id));

            HttpContext.Session.SetString("elementToDelete", id);
            return View(mapper.Map<ElementViewModel>(elementDTO));
        }

        // POST: Forms/Delete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var elementId = HttpContext.Session.GetString("elementToDelete");

            var formId = HttpContext.Session.GetString("formId");

            var existingElement = await elementService.Get(new Guid(elementId));
            var deletedElement = await elementService.Remove(existingElement);

            return RedirectToAction("Index", "Forms", new { id = formId });
        }
    }
}
