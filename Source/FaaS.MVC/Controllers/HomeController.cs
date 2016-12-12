using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using FaaS.DataTransferModels;

namespace FaaS.MVC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Mapper
        /// </summary>
        private IMapper mapper;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Home controller costructor
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">logger</param>
        public HomeController(IUserService userService, IMapper mapper, ILogger<HomeController> logger)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [Route("/welcome")]
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            UserViewModel model = new UserViewModel();
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Name", "GoogleToken", "Email", "Registered", "AvatarUrl")]UserViewModel userViewModel)
        {
            if (ModelState.IsValid) {
                // Check if the user existed before

                var userDTO = mapper.Map<UserViewModel, User>(userViewModel);
                var addedUser = await userService.Add(userDTO);

                return Redirect("/");
            }
            return View(new UserViewModel());
        }
    }
}
