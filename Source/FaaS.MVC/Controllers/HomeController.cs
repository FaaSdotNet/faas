using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IProjectService projectService;
        private IMapper mapper;

        public HomeController(IUserService userService, IProjectService projectService, IMapper mapper)
        {
            this.userService = userService;
            this.projectService = projectService;
            this.mapper = mapper;
        }

        // GET: home
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await userService.Get(new System.Guid(userId));
                ViewData["userName"] = userDTO.UserName;
                
                var projectsDTO = await projectService.GetAllForUser(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                return View(mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userName"] = null;
                return View();
            }
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await userService.Get(new System.Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await projectService.GetAllForUser(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                return View(mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userName"] = null;
                return View();
            }
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "Your contact page.";

            string userId = HttpContext.Session.GetString("userId");
            if (userId != null)
            {
                var userDTO = await userService.Get(new System.Guid(userId));
                ViewData["userName"] = userDTO.UserName;

                var projectsDTO = await projectService.GetAllForUser(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.Id.ToString(),
                    p => p.ProjectName.ToString());

                return View(mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userName"] = null;
                return View();
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        // POST: Home/LogIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("GoogleId")]UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userService.Get(user.GoogleId);

                HttpContext.Session.SetString("userId", existingUser.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Home/LogOut
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("userId");

            return RedirectToAction("Index", "Home");
        }
    }
}
