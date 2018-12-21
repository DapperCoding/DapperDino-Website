using DapperDino.Api.Models.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models
{
    public class FaqMessageIdViewModel
    {
        public DiscordMessageModel Message { get; set; }
        public int Id { get; set; }
    }
}
