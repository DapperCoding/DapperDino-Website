using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.TicketViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<TicketReactionViewModel> TicketReactions { get; set; }
    }
}
