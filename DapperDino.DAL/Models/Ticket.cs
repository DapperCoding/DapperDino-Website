using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }

        public int ApplicantId { get; set; }
        public int AssignedToId { get; set; }

        public int Priority { get; set; } = 0;
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;

        public virtual IEnumerable<TicketReaction> Reactions { get; set; }

        [ForeignKey(nameof(ApplicantId))]
        public virtual DiscordUser Applicant { get; set; }

        [ForeignKey(nameof(AssignedToId))]
        public virtual DiscordUser AssignedTo { get; set; }

    }

    public enum TicketStatus
    {
        Open = 0,
        Closed = 1
    }

    public class TicketReaction
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int FromId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public virtual Ticket Ticket { get; set; }

        
        [ForeignKey(nameof(FromId))]
        public virtual DiscordUser From { get; set; }
        
        public string Message { get; set; }
        public string MessageId { get; set; }
    }
}
