using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    [Route("/HappyToHelp/Ticket")]
    public class TicketController : BaseController
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}