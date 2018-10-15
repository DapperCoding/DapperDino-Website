using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models
{
    public class XpViewModel
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
    }
}
