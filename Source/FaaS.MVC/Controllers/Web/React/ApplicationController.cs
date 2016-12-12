using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FaaS.MVC.Controllers.Web.React
{
    [Route("/")]
    public class ApplicationController : Controller
    {

        public async Task<IActionResult> Index()
        {

            return View("~/Views/React/Application.cshtml");
        }
    }
}
