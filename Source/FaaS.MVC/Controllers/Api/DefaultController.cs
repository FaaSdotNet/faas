using AutoMapper;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace FaaS.MVC.Controllers.Api
{
    public class DefaultController : Controller
    {
        public const string RoutePrefix = "api/v1.0/";

        protected readonly IRandomIdService randomId;
        protected readonly IActionContextAccessor actionContextAccessor;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly IUrlHelperFactory urlHelperFactory;
        protected readonly IMapper mapper;

        public DefaultController(IRandomIdService randomId, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IMapper mapper)
        {
            this.randomId = randomId;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelperFactory = urlHelperFactory;
            this.mapper = mapper;
        }
    }
}
