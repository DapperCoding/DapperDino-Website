using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DapperDino.DAL.Models
{
    public class FrequentlyAskedQuestion: IEntity
    {
        public string Description { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public int? DiscordMessageId { get; set; }
        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }

        public int? ResourceLinkId { get; set; }
        [ForeignKey("ResourceLinkId")]
        public virtual ResourceLink ResourceLink { get; set; }
    }
}
