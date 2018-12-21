using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models.Discord
{
    public class TicketReactionViewModel
    {
       
        public int TicketId { get; set; }
       
        public string FromId { get; set; }

        public string Username { get; set; }

        public DiscordMessageModel DiscordMessage { get; set; }
    }
}
