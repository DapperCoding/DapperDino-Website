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
       
        public string Message { get; set; }
       
        public string MessageId { get; set; }
    }
}
