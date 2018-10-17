using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Services
{
    public class DiscordBuilder
    {
        private readonly IHubContext<DiscordBotHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public DiscordBuilder(IHubContext<DiscordBotHub> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

       
        // await _hubContext.Clients.All.SendAsync("ReceiveMessage",{}, "shit");
    }
}
