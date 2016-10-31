using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FaaS.Services;
using AutoMapper;
using FaaS.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FaaS.Services.DataTransferModels;

namespace FaaS.MVC.Controllers.Web
{
    public class FormsController : Controller
    {
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;

        public FormsController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: Forms/Index/id
        public async Task<ActionResult> Index(string id)
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

            var formDTO = await _faaSService.GetForm(id);
            ViewData["formDisplayName"] = formDTO.DisplayName;

            HttpContext.Session.SetString("formCodeName", id);

            var elementsDTO = await _faaSService.GetAllElements(formDTO);
            ViewBag.ElementDictionary = elementsDTO.ToDictionary(
                e => e.ElementCodeName.ToString(),
                e => e.Description.ToString());

            return View(_mapper.Map<FormViewModel>(formDTO));
        }

        // GET: Forms/Details/5
        public async Task<ActionResult> Details(string id)
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

            var formDTO = await _faaSService.GetForm(id);
            ViewData["formDisplayName"] = formDTO.DisplayName;

            return View(_mapper.Map<FormDetailsViewModel>(formDTO));
        }

        // GET: Forms/Create
        public async Task<ActionResult> Create()
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Index", "Home");
            }

            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;
            SelectList projects = new SelectList(await _faaSService.GetAllProjects(userDTO), "ProjectCodeName", "DisplayName");

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
                    var allForms = await _faaSService.GetAllForms();
                    var formDTO = _mapper.Map<CreateFormViewModel, Form>(form);

                    formDTO.FormCodeName = allForms.Count().ToString();
                    formDTO.Created = DateTime.Now;

                    var projectDTO = await _faaSService.GetProject(form.SelectedProjectCodeName);
                    await _faaSService.AddForm(projectDTO, formDTO);
                    return RedirectToAction("Index", "Projects", new { id = form.SelectedProjectCodeName });
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

            var existingForm = await _faaSService.GetForm(id);
            if (existingForm == null)
            {
                RedirectToAction("Index", "Projects");
            }
            ViewData["formDisplayName"] = existingForm.DisplayName;

            HttpContext.Session.SetString("formToEdit", id);
            return View(_mapper.Map<FormViewModel>(existingForm));
        }

        // POST: Forms/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormViewModel model)
        {
            var formCodeName = HttpContext.Session.GetString("formToEdit");

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;

            try
            {
                var formDTO = _mapper.Map<FormViewModel, Form>(model);
                var updatedForm = await _faaSService.UpdateForm(formDTO);

                return RedirectToAction("Index", "Forms", new { id = updatedForm.FormCodeName });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Forms/Delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserCodeName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            var projectsDTO = await _faaSService.GetAllProjects(existingUser);
            ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

            var projectCodeName = HttpContext.Session.GetString("projectCodeName");
            var projectDTO = await _faaSService.GetProject(projectCodeName);
            ViewData["projectDisplayName"] = projectDTO.DisplayName;

            var formsDTO = await _faaSService.GetAllForms(projectDTO);
            ViewBag.FormDictionary = formsDTO.ToDictionary(
                f => f.FormCodeName.ToString(),
                f => f.DisplayName.ToString());

            var formDTO = await _faaSService.GetForm(id);
            ViewData["formDisplayName"] = formDTO.DisplayName;

            HttpContext.Session.SetString("formToDelete", id);
            return View(_mapper.Map<FormViewModel>(formDTO));
        }

        // POST: Forms/Delete
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var formToDelete = HttpContext.Session.GetString("formToDelete");

            var projectCodeName = HttpContext.Session.GetString("projectCodeName");

            var existingForm = await _faaSService.GetForm(formToDelete);
            var deletedForm = await _faaSService.RemoveForm(existingForm);

            return RedirectToAction("Index", "Projects", new { id = projectCodeName});
        }
    }
}