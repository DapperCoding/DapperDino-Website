using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Suggestion
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public SuggestionTypes Type { get; set; }
        public SuggestionStatus Status { get; set; } = SuggestionStatus.NotLookedAt;
        public int? DiscordUserId { get; set; }


        [ForeignKey("DiscordUserId")]
        public virtual DiscordUser DiscordUser { get; set; }

        public virtual IEnumerable<SuggestionReaction> Reactions { get; set; }

    }

    public enum SuggestionTypes
    {
        Bot = 0,
        Website = 1,
        General = 2,
        YouTube = 3,
        Undecided = 4
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
