using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Controllers;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace DapperDino.Controllers
{
    public class TemporaryController : BaseController
    {
        private readonly HubConnection _connection;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        public TemporaryController(IHubContext<DiscordBotHub> hubContext)
        {
         
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Test()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "test", "shit");

            return Ok();
        }

        

    }
}