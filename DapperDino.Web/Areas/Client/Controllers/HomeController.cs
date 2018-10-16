using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Client.Controllers
{
    [Route("/Client")]
    public class HomeController: BaseController
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
