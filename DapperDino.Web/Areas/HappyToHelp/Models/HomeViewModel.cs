using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.HappyToHelp.Models
{
    public class HomeViewModel
    {


        public int AmountOfSuggestions { get; set; } = 10;
        public int AmountOfHandledSuggestions { get; set; } = 10;
        public int AmountOfNotLookedAtSuggestions { get; set; } = 10;

        public int AmountOfTickets { get; set; } = 10;
        public int AmountOfOpenTickets { get; set; } = 10;
        public int AmountOfClosedTickets { get; set; } = 10;
    }
}
