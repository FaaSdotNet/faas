using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web
{
    public class UsersController : Controller
    {
        private readonly IFaaSService _faaSService;
        private IMapper _mapper;
        private static User _superUser = new User();

        public UsersController(IFaaSService faaSService, IMapper mapper)
        {
            _faaSService = faaSService;
            _mapper = mapper;
        }

        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            var usersDTO = await _faaSService.GetAllUsers();
            return View(_mapper.Map<IEnumerable<UserViewModel>>(usersDTO));
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var newUser = new CreateUserViewModel();

            return View(newUser);
        }

        public async Task<IActionResult> Details()
        {
            var userCodeName = HttpContext.Session.GetString("userCodeName");
            var existingUser = await _faaSService.GetUserName(userCodeName);

            ViewData["userDisplayName"] = existingUser.DisplayName;

            return View(_mapper.Map<UserDetailsViewModel>(existingUser));
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserCodeName", "DisplayName", "GoogleId", "Registered")]CreateUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userDTO = _mapper.Map<CreateUserViewModel, User>(user);
                var addedUser = await _faaSService.AddUser(userDTO);

                HttpContext.Session.SetString("userCodeName", addedUser.UserCodeName);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
    }
}