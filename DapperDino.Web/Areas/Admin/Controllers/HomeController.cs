using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Admin.Controllers
{
    // Admin home controller
    [Route("Admin")]
    public class HomeController : BaseController
    {

        // Default page
        [Route("")]
        public IActionResult Index()
        {

            // Simply return the view
            return View();
        }
    }
}