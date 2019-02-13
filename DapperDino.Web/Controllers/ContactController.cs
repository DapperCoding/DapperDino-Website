using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Jobs;
using DapperDino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Controllers
{
    [Route("/Contact")]
    public class ContactController : BaseControllerBase
    {
        private IHubContext<DiscordBotHub> hubContext;

        public ContactController(IHubContext<DiscordBotHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [Route("")]
        public IActionResult Index(ContactViewModel viewModel = null)
        {
            if (viewModel == null) viewModel = new ContactViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Route("Post")]
        public IActionResult Post(ContactViewModel viewModel)
        {
            if (!TryValidateModel(viewModel)) return RedirectToAction("Index", viewModel);

            hubContext.Clients.All.SendAsync("ContactRequest", viewModel);

            return RedirectToAction("ContactSoon");
        }

        [Route("ContactSoon")]
        public IActionResult ContactSoon()
        {
            return View();
        }
    }
}