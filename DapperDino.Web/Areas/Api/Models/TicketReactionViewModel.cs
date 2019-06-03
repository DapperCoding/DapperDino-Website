using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Api.Models
{
    public class TicketUserInformation
    {
        public UserStatus Status { get; set; }
        public string Username { get; set; }
        
    }

    public class TicketReactionUserInformation
    {
        public UserStatus Status { get; set; }
        public string AvatarUrl { get; set; }
        public string DiscordId { get; set; }
        public string Username { get; set; }

    }
}
