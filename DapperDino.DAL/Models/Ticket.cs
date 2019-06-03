using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Ticket:IEntity
    {
        public string Description { get; set; }
        [Required]
        public string Subject { get; set; }

        public int ApplicantId { get; set; }

        public int Priority { get; set; } = 0;
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketCategory Category { get; set; } = TicketCategory.Undecided;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;

        public int? LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Proficiency Language { get; set; }

        public int? FrameworkId { get; set; }
        [ForeignKey("FrameworkId")]
        public virtual Proficiency Framework { get; set; }

        public virtual IEnumerable<TicketReaction> Reactions { get; set; }

        [ForeignKey("ApplicantId")]
        public virtual DiscordUser Applicant { get; set; }

        public virtual List<TicketUser> Assignees { get; set; }

    }

    public enum TicketStatus
    {
        Open = 0,
        Closed = 1,
        InProgress = 2
    }

    public enum TicketCategory
    {
        DiscordBots = 0,
        Unity = 1,
        Python = 2,
        Web = 3,
        CSharp = 4,
        Undecided = 5
    }

    public class TicketReaction:IEntity
    {
        public int TicketId { get; set; }
        public int FromId { get; set; }
        public int DiscordMessageId { get; set; }

        [ForeignKey("TicketId")]
        public  Ticket Ticket { get; set; }
        
        [ForeignKey("FromId")]
        public DiscordUser From { get; set; }

        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }
    }
}
