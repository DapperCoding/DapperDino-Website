using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.HappyToHelp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    // Admin home controller
    [Route("HappyToHelp")]
    public class HomeController : BaseController
    {

        // Default page
        [Route("")]
        public IActionResult Index()
        {

            // Simply return the view
            return View(new HomeViewModel());
        }
    }
}