using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordAttachment : IEntity
    {
        public string DiscordAttachmentId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string Url { get; set; }
        public string ProxyUrl { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public int DiscordMessageId { get; set; }

        [ForeignKey("DiscordMessageId")]
        public virtual DiscordMessage DiscordMessage { get; set; }
    }
}
