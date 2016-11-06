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
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;

        public ElementsController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: Elements/Create
        public async Task<IActionResult> Create()
        {
            var newElement = new CreateElementViewModel();

            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await _faaSService.GetUser(new Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                string projectId = HttpContext.Session.GetString("projectId");
                var projectDTO = await _faaSService.GetProject(new Guid(projectId));
                ViewData["projectName"] = projectDTO.ProjectName;

                var formsDTO = await _faaSService.GetAllForms(projectDTO);
                ViewBag.FormDictionary = formsDTO.ToDictionary(
                    f => f.Id.ToString(),
                    f => f.FormName.ToString());

                string formId = HttpContext.Session.GetString("formId");
                var formDTO = await _faaSService.GetForm(new Guid(formId));
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
            var existingUser = await _faaSService.GetUser(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            string projectId = HttpContext.Session.GetString("projectId");
            var projectDTO = await _faaSService.GetProject(new Guid(projectId));
            ViewData["projectId"] = projectDTO.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            string formId = HttpContext.Session.GetString("formId");
            var formDTO = await _faaSService.GetForm(new Guid(formId));

            ViewData["formName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var existingElement = await _faaSService.GetElement(new Guid(id));

            return View(_mapper.Map<ElementDetailsViewModel>(existingElement));
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
                    var allElements = await _faaSService.GetAllElements();
                    var elementDTO = _mapper.Map<CreateElementViewModel, Element>(element);

                    string formId = HttpContext.Session.GetString("formId");
                    var formDTO = await _faaSService.GetForm(new Guid(formId));
                    await _faaSService.AddElement(formDTO, elementDTO);
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
            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(userDTO);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            string projectId = HttpContext.Session.GetString("projectId");
            var projectDTO = await _faaSService.GetProject(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            string formId = HttpContext.Session.GetString("formId");
            var formDTO = await _faaSService.GetForm(new Guid(formId));
            ViewData["formName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var existingElement = await _faaSService.GetElement(new Guid(id));
            if (existingElement == null)
            {
                RedirectToAction("Index", "Projects");
            }

            HttpContext.Session.SetString("elementToEdit", id);
            return View(_mapper.Map<ElementViewModel>(existingElement));
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
            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;

            try
            {
                var elementDTO = _mapper.Map<ElementViewModel, Element>(model);
                elementDTO.Id = new Guid(elementId);
                var updatedElement = await _faaSService.UpdateElement(elementDTO);

                string formId = HttpContext.Session.GetString("formId");
                var formDTO = await _faaSService.GetForm(new Guid(formId));

                return RedirectToAction("Index", "Forms", new { id = formId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Elements/Delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await _faaSService.GetUser(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

            var projectId = HttpContext.Session.GetString("projectId");
            var projectDTO = await _faaSService.GetProject(new Guid(projectId));
            ViewData["projectName"] = projectDTO.ProjectName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.Id.ToString(),
                f => f.FormName.ToString());

            var formId = HttpContext.Session.GetString("formId");
            var formDTO = await _faaSService.GetForm(new Guid(formId));
            ViewData["formDisplayName"] = formDTO.FormName;
            ViewData["formId"] = formDTO.Id;

            var elementDTO = await _faaSService.GetElement(new Guid(id));

            HttpContext.Session.SetString("elementToDelete", id);
            return View(_mapper.Map<ElementViewModel>(elementDTO));
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

            var existingElement = await _faaSService.GetElement(new Guid(elementId));
            var deletedElement = await _faaSService.RemoveElement(existingElement);

            return RedirectToAction("Index", "Forms", new { id = formId });
        }
    }
}
