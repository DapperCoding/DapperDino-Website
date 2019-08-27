using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{
    public class CustomBotForm:IEntity
    {
        public int DiscordId { get; set; }
        [ForeignKey("DiscordId")]
        public DiscordUser DiscordUser { get; set; }
        public string Functionalities { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        
    }
}
