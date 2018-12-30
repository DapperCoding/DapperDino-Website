using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordMessage:IEntity
    {
        [Required]
        public string MessageId { get; set; }
        public string GuildId { get; set; }
        public string ChannelId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public bool IsEmbed { get; set; }
        
        // If true, GuildId and ChannelId are required
        public bool IsDm { get; set; }

        public string ImageLink { get; set; }

        public int DiscordUserId { get; set; }
        [ForeignKey("DiscordUserId")]
        public DiscordUser DiscordUser { get; set; }
    }
}
