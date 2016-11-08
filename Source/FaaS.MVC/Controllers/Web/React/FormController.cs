using System.Threading.Tasks;
using AutoMapper;
using FaaS.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaaS.MVC.Controllers.Web.React
{
    public class FormController : Controller
    {
        /// <summary>
        /// Form service
        /// </summary>
        private readonly IFormService formService;
        private IMapper _mapper;

        public FormController(IFormService formService, IMapper mapper)
        {
            this.formService = formService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {

            return View("~/Views/React/Form.cshtml");
        }
    }
}
