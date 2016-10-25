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
                ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

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
                ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

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
                ViewBag.ProjectNames = projectsDTO.Select(p => p.DisplayName).ToList();

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
    }
}
