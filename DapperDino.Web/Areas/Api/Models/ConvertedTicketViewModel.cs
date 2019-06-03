using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DapperDino.Areas.Api.Models
{
    public class ConvertedTicketViewModel
    {
        public Ticket Ticket {get;set; }
        public IEnumerable<TicketReactionUserInformation> ReactionInformation { get; set; }
    }
}
