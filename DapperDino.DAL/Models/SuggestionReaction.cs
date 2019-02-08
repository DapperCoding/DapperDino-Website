using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class SuggestionReaction : IEntity
    {
        public int SuggestionId { get; set; }
        public int FromId { get; set; }
        public int DiscordMessageId { get; set; }

        [ForeignKey("SuggestionId")]
        public virtual Suggestion Suggestion { get; set; }

        [ForeignKey("FromId")]
        public virtual DiscordUser From { get; set; }

        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }
    }
}
