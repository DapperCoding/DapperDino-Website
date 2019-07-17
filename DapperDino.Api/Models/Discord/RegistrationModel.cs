using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models
{
    public class RegistrationModel
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public bool IsHappyToHelp { get; set; } = false;
    }
}
