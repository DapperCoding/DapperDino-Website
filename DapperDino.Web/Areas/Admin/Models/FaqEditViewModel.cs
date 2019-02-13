using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Models;

namespace DapperDino.Areas.Admin.Models
{
    public class FaqEditViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public int? ResourceLinkId { get; set; }
        public ResourceLinkViewModel ResourceLink { get; set; }
    }

    public class ResourceLinkViewModel
    {
        public int? Id { get; set; }
        public string Link { get; set; }
        public string DisplayName { get; set; }
    }
}
