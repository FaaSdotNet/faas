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
using AutoMapper;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]
    public class LoginController : DefaultController
    {
        public const string RouteController = "login";

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        private readonly ILogger<UsersController> logger;

        public LoginController(IUserService userService, IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IMapper mapper, ILogger<UsersController> logger) : base(null, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.userService = userService;
            this.logger = logger;
        }

        // POST login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Post([FromBody]UserViewModel userViewModel)
        {
            try
            {
                    var existingUser = await userService.GetByToken(userViewModel.GoogleToken);
                    if (existingUser == null)
                    {
                        return NotFound("User not found : " + userViewModel.GoogleToken);
                    }

                    logger.LogDebug("[LOGIN] User: " + existingUser);
                    return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
