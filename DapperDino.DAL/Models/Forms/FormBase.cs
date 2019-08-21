using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{
    public abstract class FormBase:IEntity
    {
        public int DiscordId { get; set; }
        [ForeignKey("DiscordId")]
        public DiscordUser DiscordUser { get; set; }
        public string Motivation { get; set; }

        // Don't forget to add the reactions to your implementation
    }

    public abstract class FormReply<T>:IEntity where T:FormBase
    {
        public int DiscordMessageId { get; set; }
        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public T Form { get; set; }

    }
}
