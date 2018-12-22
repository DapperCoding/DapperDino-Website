using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class TicketUser
    {
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public int DiscordUserId { get; set; }
        public virtual DiscordUser DiscordUser { get; set; }

    }
}
