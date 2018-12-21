using DapperDino.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordUser: IEntity
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }

        public virtual List<TicketUser> TicketUsers { get; set; }
    }
}
