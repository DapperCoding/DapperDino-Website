using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Api.Models
{
    public class NotSentTicketMessage
    {
        public string Message { get; set; }
        public int TicketId { get; set; }
        public int? FromId { get; set; }
        public string Username { get; set; }
        public string DiscordId { get; set; }
    }
}
