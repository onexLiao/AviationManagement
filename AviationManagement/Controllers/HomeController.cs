using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AviationManagement.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("/swagger/", true);
        }
    }
}