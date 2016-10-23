using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FaaS.MVC.Models;
using FaaS.Services;
using FaaS.Services.DataTransferModels;
using Microsoft.AspNetCore.Mvc;

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

        // GET: Products
        [ActionName("Index")]
        public async Task<IActionResult> List()
        {
            var usersDTO = await _faaSService.GetAllUsers();
            return View(_mapper.Map<IEnumerable<ProjectViewModel>>(usersDTO));
        }
    }
}