using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Poll:IEntity
    {
        public string Name { get; set; }
        public List<Vote> Votes { get; set; }
        public List<PollOption> Options { get; set; }
        public int CreatedByDiscordId { get; set; }
    }

    public class Vote:IEntity
    {
        public int DiscordUserId { get; set; }
        public int PollId { get; set; }
        [ForeignKey("PollId")]
        public Poll Poll { get; set; }
        [ForeignKey("DiscordUserId")]
        public DiscordUser DiscordUser { get; set; }
    }

    public class PollOption: IEntity
    {
        public string Value { get; set; }
        public int PollId { get; set; }
        [ForeignKey("PollId")]
        public Poll Poll { get; set; }
    }
}
