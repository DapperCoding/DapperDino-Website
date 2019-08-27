using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models.Ideas
{
    public class LinkBase:IEntity
    {
        public string Link { get; set; }
        public string Explanation { get; set; }
    }
}
