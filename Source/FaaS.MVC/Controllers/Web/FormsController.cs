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

            return View(_mapper.Map<FormViewModel>(formDTO));

        }

        // GET: Forms/Details/5
        public async Task<ActionResult> Details(string formCodeName)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Index", "Home");
            }

            var existingUser = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = existingUser.DisplayName;
            var form = await _faaSService.GetForm(formCodeName);
           
            return View(_mapper.Map<FormDetailsViewModel>(form));
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
                    return RedirectToAction("Forms", "Forms" ,new { projectCodeName = form.SelectedProjectCodeName });
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

            var existingForm = await _faaSService.GetForm(id);
            if (existingForm == null)
            {
                RedirectToAction("Index", "Projects");
            }

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

                return RedirectToAction("Forms", "Forms", new { projectCodeName = updatedForm.Project.ProjectCodeName });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Projects");
            }
        }

        // GET: Forms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Forms/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}