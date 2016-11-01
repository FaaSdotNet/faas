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

            var formDTO = await _faaSService.GetForm(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

            HttpContext.Session.SetString("formId", id);

            var elementsDTO = await _faaSService.GetAllElements(formDTO);
            ViewBag.ElementDictionary = elementsDTO.ToDictionary(
                e => e.Id.ToString(),
                e => e.Description.ToString());

            return View(_mapper.Map<FormViewModel>(formDTO));
        }

        // GET: Forms/Details/Id
        public async Task<ActionResult> Details(string id)
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

            var formDTO = await _faaSService.GetForm(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

            return View(_mapper.Map<FormDetailsViewModel>(formDTO));
        }

        // GET: Forms/Create
        public async Task<ActionResult> Create()
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                RedirectToAction("Index", "Home");
            }

            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userName"] = userDTO.UserName;
            SelectList projects = new SelectList(await _faaSService.GetAllProjects(userDTO), "Id", "ProjectName");

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

                    formDTO.Created = DateTime.Now;

                    var projectDTO = await _faaSService.GetProject(form.SelectedProjectId);
                    await _faaSService.AddForm(projectDTO, formDTO);
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

            var existingForm = await _faaSService.GetForm(new Guid(id));
            if (existingForm == null)
            {
                RedirectToAction("Index", "Projects");
            }
            ViewData["formName"] = existingForm.FormName;

            HttpContext.Session.SetString("formToEdit", id);
            return View(_mapper.Map<FormViewModel>(existingForm));
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
            var userDTO = await _faaSService.GetUser(new Guid(userId));
            ViewData["userDisplayName"] = userDTO.UserName;

            try
            {
                var formDTO = _mapper.Map<FormViewModel, Form>(model);
                var updatedForm = await _faaSService.UpdateForm(formDTO);

                return RedirectToAction("Index", "Forms", new { id = updatedForm.Id });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Forms/Delete/Id
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

            var formDTO = await _faaSService.GetForm(new Guid(id));
            ViewData["formName"] = formDTO.FormName;

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
            var formId = HttpContext.Session.GetString("formToDelete");

            var projectId = HttpContext.Session.GetString("projectId");

            var existingForm = await _faaSService.GetForm(new Guid(formId));
            var deletedForm = await _faaSService.RemoveForm(existingForm);

            return RedirectToAction("Index", "Projects", new { id = projectId});
        }
    }
}