using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Client.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Suggestion> Suggestions { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
        public bool ConnectedDiscordAccount { get; set; }

    }
}
