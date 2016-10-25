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

        // GET: Forms
        public async Task<ActionResult> Forms(string projectCodeName)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Login", "Users");
            }

            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            var projects = await _faaSService.GetAllProjects(userDTO);

            ViewData["userDisplayName"] = userDTO.DisplayName;
            var project = await _faaSService.GetProject(projectCodeName);
            var forms = (await _faaSService.GetAllForms(project)).ToList();

            return View(_mapper.Map<IEnumerable<FormViewModel>>(forms));

        }

        // GET: Forms/Details/5
        public async Task<ActionResult> Details(string formCodeName)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Login", "Users");
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
                RedirectToAction("Login", "Users");
            }

            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;
            SelectList projects = new SelectList(await _faaSService.GetAllProjects(userDTO), "ProjectCodeName", "DisplayName");

            CreateFormViewModel model = new CreateFormViewModel
            {
                ProjectList = projects //new List<Project>(await _faaSService.GetAllProjects(userDTO))
            };

            return View(model);
        }

        // POST: Forms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateFormViewModel model)
        {
            Form formDTO = new Form
            {
                Created = DateTime.Now,
                Description = model.Description,
                DisplayName = model.DisplayName,
                FormCodeName = model.DisplayName,   // TODO set proper codename
            };

            try
            {
                var projectDTO = await _faaSService.GetProject(model.SelectedProjectCodeName);
                await _faaSService.AddForm(projectDTO, formDTO);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "Home");   // error page or something
            }
        }

        // GET: Forms/Edit/5
        public async Task<ActionResult> Edit(string formCodeName)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Login", "Users");
            }
            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;

            var form = await _faaSService.GetForm(formCodeName);
            if (form == null)
            {
                RedirectToAction("Index", "Projects");
            }

            CreateFormViewModel model = new CreateFormViewModel
            {
                Description = form.Description,
                DisplayName = form.DisplayName,
                FormCodeName = form.FormCodeName
            };

            return View(model);
        }

        // POST: Forms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateFormViewModel model)
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName == null)
            {
                RedirectToAction("Login", "Users");
            }
            var userDTO = await _faaSService.GetUserCodeName(userCodeName);
            ViewData["userDisplayName"] = userDTO.DisplayName;

            try
            {
                var form = await _faaSService.GetForm(model.FormCodeName);
                if (form == null)
                {
                    RedirectToAction("Index", "Projects");
                }
                
                form.Description = model.Description;
                form.DisplayName = model.DisplayName;

                await _faaSService.UpdateForm(form);

                return RedirectToAction("Forms", "Forms", new { projectCodeName = form.Project.ProjectCodeName });
            }
            catch
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