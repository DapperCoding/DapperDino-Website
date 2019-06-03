using DapperDino.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordUser: IEntity
    {
        public string DiscordId { get; set; }
        public string Discriminator { get; set; }
        public bool IsBot { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string DefaultAvatarUrl { get; }
        public string AvatarUrl { get; }
        public int Xp { get; set; }
        public int Level { get; set; }

        public virtual List<TicketUser> TicketUsers { get; set; }
        public virtual List<DiscordUserProficiency> Proficiencies { get; set; }
    }
}
