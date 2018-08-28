using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DapperDino.DAL.Models;
using DapperDino.Models.TicketViewModels;

namespace DapperDino.Profiles
{
    public class TicketProfile:Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketViewModel>();
            CreateMap<TicketReaction, TicketReactionViewModel>();

        }
    }
}
