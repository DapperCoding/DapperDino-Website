using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models
{
    public class TicketSystemTicket
    {
        public Ticket Ticket { get; set; }
        public DSharpPlus.Entities.DiscordMessage Reactions { get; set; }
    }
}
