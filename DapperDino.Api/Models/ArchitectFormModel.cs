using DapperDino.DAL.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Models
{

    public class ArchitectFormModel:ArchitectForm
    {
        public new string DiscordId { get; set; }
    }
}
