using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordUser
    {
        public int Id { get; set; }
        public int DiscordId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
