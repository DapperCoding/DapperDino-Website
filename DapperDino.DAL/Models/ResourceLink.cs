using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class ResourceLink:IEntity
    {
        public string DisplayName { get; set; }
        public string Link { get; set; }
    }
}
