using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{
    public class CustomBotForm : IEntity
    {
        public int DiscordId { get; set; }
        [ForeignKey("DiscordId")]
        public DiscordUser DiscordUser { get; set; }
        public string Functionalities { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public IEnumerable<CustomBotFormReply> Replies { get; set; }
        public CustomBotFormStatus Status { get; set; }
    }

    public enum CustomBotFormStatus
    {
        NotLookedAt = 0,
        TalkingTo = 1,
        NoDeal = 2,
        Deal = 3,
        InProgress = 4,
        Abandoned = 5,
        Done = 6
    }

    public class CustomBotFormModel : CustomBotForm
    {
        public new string DiscordId { get; set; }
    }

    public class CustomBotFormReply : IEntity
    {
        public int DiscordMessageId { get; set; }
        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public CustomBotForm Form { get; set; }
    }

    public class CustomBotFormReplyModel : CustomBotFormReply
    {
        public string DiscordId { get; set; }
    }
}
