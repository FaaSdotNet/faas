using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

                string projectCodeName = HttpContext.Session.GetString("projectCodeName");
                var projectDTO = await _faaSService.GetProject(projectCodeName);
                ViewData["projectDisplayName"] = projectDTO.DisplayName;

                var formsDTO = await _faaSService.GetAllForms(projectDTO);
                ViewBag.FormDictionary = formsDTO.ToDictionary(
                    f => f.FormCodeName.ToString(),
                    f => f.DisplayName.ToString());

                string formCodeName = HttpContext.Session.GetString("formCodeName");
                var formDTO = await _faaSService.GetForm(formCodeName);
                ViewData["formDisplayName"] = formDTO.DisplayName;
                ViewData["formCodeName"] = formDTO.FormCodeName;

                return View(newElement);
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View(newElement);
            }
        }

        // GET: Element/Details
        public async Task<IActionResult> Details(string id)
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

            string projectCodeName = HttpContext.Session.GetString("projectCodeName");
            var projectDTO = await _faaSService.GetProject(projectCodeName);
            ViewData["projectDisplayName"] = projectDTO.DisplayName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.FormCodeName.ToString(),
                f => f.DisplayName.ToString());

            string formCodeName = HttpContext.Session.GetString("formCodeName");
            var formDTO = await _faaSService.GetForm(formCodeName);

            ViewData["formDisplayName"] = formDTO.DisplayName;
            ViewData["formCodeName"] = formDTO.FormCodeName;

            var existingElement = await _faaSService.GetElement(id);

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

                    elementDTO.ElementCodeName = allElements.Count().ToString();
                    elementDTO.DisplayName = "test";

                    string formCodeName = HttpContext.Session.GetString("formCodeName");
                    var formDTO = await _faaSService.GetForm(formCodeName);
                    await _faaSService.AddElement(formDTO, elementDTO);
                    return RedirectToAction("Index", "Forms", new { id = formCodeName });
                }
                catch
                {
                    return RedirectToAction("Index", "Home");   // error page or something
                }
            }
            return View(element);
        }

        // GET: Elements/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;

            var projectsDTO = await _faaSService.GetAllProjects(userDTO);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

            string projectCodeName = HttpContext.Session.GetString("projectCodeName");
            var projectDTO = await _faaSService.GetProject(projectCodeName);
            ViewData["projectDisplayName"] = projectDTO.DisplayName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.FormCodeName.ToString(),
                f => f.DisplayName.ToString());

            string formCodeName = HttpContext.Session.GetString("formCodeName");
            var formDTO = await _faaSService.GetForm(formCodeName);
            ViewData["formDisplayName"] = formDTO.DisplayName;
            ViewData["formCodeName"] = formDTO.FormCodeName;

            var existingElement = await _faaSService.GetElement(id);
            if (existingElement == null)
            {
                RedirectToAction("Index", "Projects");
            }

            HttpContext.Session.SetString("elementToEdit", id);
            return View(_mapper.Map<ElementViewModel>(existingElement));
        }

        // POST: Elements/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ElementViewModel model)
        {
            var elementCodeName = HttpContext.Session.GetString("elementToEdit");

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;

            try
            {
                var elementDTO = _mapper.Map<ElementViewModel, Element>(model);
                var updatedElement = await _faaSService.UpdateElement(elementDTO);

                string formCodeName = HttpContext.Session.GetString("formCodeName");
                var formDTO = await _faaSService.GetForm(formCodeName);

                return RedirectToAction("Index", "Forms", new { id = formCodeName });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Projects");
            }
        }
    }
}
