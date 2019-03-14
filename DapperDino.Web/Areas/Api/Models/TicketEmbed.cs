using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Api.Models
{
    public class TicketEmbed
    {
        public Ticket Ticket { get; set; }
        public DiscordUser User { get; set; }
    }

    public class CloseTicketEmbed : TicketEmbed
    {
    }

    public class AcceptTicketEmbed : TicketEmbed
    {
    }
}
