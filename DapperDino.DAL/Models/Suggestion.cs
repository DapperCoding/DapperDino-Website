using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public SuggestionTypes Type { get; set; } = SuggestionTypes.Bot;
        public SuggestionStatus Status { get; set; } = SuggestionStatus.NotLookedAt;

        public int DiscordUserId { get; set; }

        [ForeignKey(nameof(DiscordUserId))]
        public virtual DiscordUser DiscordUser { get; set; }

    }

    public enum SuggestionTypes
    {
        Bot = 0,
        Website = 1,
        General = 2,
        YouTube = 3
    }

    public enum SuggestionStatus
    {
        Abandoned = 0,
        WorkInProgress = 1,
        InConsideration = 2,
        Completed = 3,
        Future = 4,
        NotLookedAt = 5
    }
}
