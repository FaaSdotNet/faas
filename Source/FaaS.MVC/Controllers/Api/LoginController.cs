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

        private readonly IFaaSService service;
        private readonly ILogger<UsersController> logger;

        public LoginController(IFaaSService service, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<UsersController> logger)
        {
            this.service = service;
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
                    var existingUser = await service.GetUser(user.GoogleId);
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
