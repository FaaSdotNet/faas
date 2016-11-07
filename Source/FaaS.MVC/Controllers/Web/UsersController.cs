using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.DataTransferModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private IMapper mapper;
        private static User superUser = new User();

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            var usersDTO = await userService.GetAll();
            return View(mapper.Map<IEnumerable<UserViewModel>>(usersDTO));
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var newUser = new CreateUserViewModel();

            return View(newUser);
        }

        public async Task<IActionResult> Details()
        {
            var userId = HttpContext.Session.GetString("userId");
            var existingUser = await userService.Get(new Guid(userId));

            ViewData["userName"] = existingUser.UserName;

            return View(mapper.Map<UserDetailsViewModel>(existingUser));
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName", "GoogleId", "Registered")]CreateUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userDTO = mapper.Map<CreateUserViewModel, User>(user);
                var addedUser = await userService.Add(userDTO);

                HttpContext.Session.SetString("userId", addedUser.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
    }
}