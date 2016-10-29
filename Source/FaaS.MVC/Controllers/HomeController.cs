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
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;

        public HomeController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        // GET: home
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;
                
                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

                return View(_mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View();
            }
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

                return View(_mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userDisplayName"] = null;
                return View();
            }
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["Message"] = "Your contact page.";

            string userCodeName = HttpContext.Session.GetString("userCodeName");
            if (userCodeName != null)
            {
                var userDTO = await _faaSService.GetUserCodeName(userCodeName);
                ViewData["userDisplayName"] = userDTO.DisplayName;

                var projectsDTO = await _faaSService.GetAllProjects(userDTO);
                ViewBag.ProjectDictionary = projectsDTO.ToDictionary(
                    p => p.ProjectCodeName.ToString(),
                    p => p.DisplayName.ToString());

                return View(_mapper.Map<UserViewModel>(userDTO));
            }
            else
            {
                ViewData["userDisplayName"] = null;
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
                var existingUser = await _faaSService.GetUserGoogleId(user.GoogleId);

                HttpContext.Session.SetString("userCodeName", existingUser.UserCodeName);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Home/LogOut
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("userCodeName");

            return RedirectToAction("Index", "Home");
        }
    }
}
