using System;
using System.Threading.Tasks;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class LoginController : Controller
    {
        public const string RoutePrefix = "api/v1.0/";
        public const string RouteController = "login";

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        private readonly ILogger<UsersController> logger;

        public LoginController(IUserService userService, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        // POST projects
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("GoogleId")][FromBody]UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await userService.Get(user.GoogleId);
                    if (existingUser == null)
                    {
                        return NotFound("User not found : " + user.GoogleId);
                    }

                    HttpContext.Session.SetString("userId", existingUser.Id.ToString());
                    return Ok(HttpContext.Session);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Unauthorized();
        }

    }
}
