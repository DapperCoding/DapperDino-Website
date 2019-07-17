using DapperDino.Models;

namespace DapperDino.Areas.HappyToHelp.Models
{
    internal class ResourceLinkViewModel : DapperDino.Models.ResourceLinkViewModel
    {
        public string Link { get; set; }
        public string DisplayName { get; set; }
        public int? Id { get; set; }
    }
}