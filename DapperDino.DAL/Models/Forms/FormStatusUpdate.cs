using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{
    public class FormStatusUpdate:IEntity
    {
        public DateTime Timestamp { get; set; }
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public FormStatusUpdateStatus Status { get; set; }
        public int DiscordId { get; set; }
        [ForeignKey("DiscordId")]
        public DiscordUser DiscordUser { get; set; }
        public int FormId { get; set; }
        public string FormType { get; set; }
        public string Reason { get; set; }
    }

    public class FormStatusUpdateStatus : IEntity
    {
        public string Name { get; set; }
    }


    public class FormStatusUpdateModel : FormStatusUpdate
    {
        public new string Timestamp { get; set; }
        public new string DiscordId { get; set; }
    }

    public class FormStatusUpdateStatusModel:FormStatusUpdateStatus
    {
    }
}
