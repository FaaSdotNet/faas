using AutoMapper;
using FaaS.DataTransferModels;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Api
{
    [Route(RoutePrefix + RouteController)]

    public class UsersController : DefaultController
    {
        public const string RouteController = "users";

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService userService;

        private readonly ILogger<UsersController> logger;


        public UsersController(IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IMapper mapper, IUserService userService, ILogger<UsersController> logger) : base(randomId, actionContextAccessor, httpContextAccessor, urlHelperFactory, mapper)
        {
            this.userService = userService;
            this.logger = logger;
        }


        // GET users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery(Name = "limit")]int limit,
            [FromQuery(Name = "attributes")]string[] attributes)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }


            var users = await userService.GetAll();


            // Apply limit
            if (limit > 0)
            {
                users = users.Take(limit).ToArray();
            }

            // Select only given fields
            if (attributes != null && attributes.Any())
            {
                users = users.Select(user =>
                {
                    var projection = new User();

                    foreach (var attribute in attributes)
                    {
                        projection.GetType()
                            .GetProperty(attribute)
                            .SetValue(projection, user.GetType().GetProperty(attribute).GetValue(user));
                    }

                    return projection;
                }).ToArray();
            }

            logger.LogInformation($"Retrieved {users.Length} users.");
            return Ok(users);
        }

        // GET users/{id}/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                Response.StatusCode = 401;
                return Unauthorized();
            }

            var user = await userService.Get(id);

            if (user == null)
            {
                return NotFound("Cannot find user with id = " + id.ToString());
            }

            return Ok(user);
        }


        // POST projects
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserViewModel user)
        {
            try
            {
                user.Registered = DateTime.Now;
                var userDto = mapper.Map<CreateUserViewModel, User>(user);
                var result = await userService.Add(userDto);

                var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                var newUrl = new Uri(urlHelper.Action("GetUser", "Users", new
                {
                    id = user.Id,
                }, httpContextAccessor.HttpContext.Request.Scheme));
                logger.LogInformation("Generated new user with name " + userDto.UserName);

                return Created(newUrl, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [HttpPatch]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserViewModel user)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var userDto = mapper.Map<UserViewModel, User>(user);
                var result = await userService.Update(userDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(userId))
                {
                    Response.StatusCode = 401;
                    return Unauthorized();
                }

                var user = await userService.Get(id);
                var result = await userService.Remove(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
