using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models.Discord
{
    public class DiscordMessageModel
    {
        public string MessageId { get; set; }
        public string ImageLink { get; set; }
        public string UserId { get; set; }
        public string GuildId { get; set; }
        public string ChannelId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public bool IsEmbed { get; set; }
        [Required]
        public bool IsDm { get; set; }
    }
}
