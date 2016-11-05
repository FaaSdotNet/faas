using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.v3;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class LoginController : Controller
    {
        public const string RoutePrefix = "api/v1.0/";
        public const string RouteController = "login";

        private readonly IFaaSService service;
        private readonly IRandomIdService randomId;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly ILogger<UsersController> logger;

        public LoginController(IFaaSService service, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<UsersController> logger)
        {
            this.service = service;
            this.randomId = randomId;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelperFactory = urlHelperFactory;
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
