using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models
{
    public class CompactXpViewModel
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
    }

    public class XpViewModel : CompactXpViewModel
    {
        public bool LevelledUp { get; set; }
    }
}
