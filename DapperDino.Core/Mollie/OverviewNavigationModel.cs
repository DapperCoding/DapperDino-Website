using Mollie.Api.Models.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Core.Mollie
{
    public class OverviewNavigationLinksModel
    {
        public UrlLink Previous { get; set; }
        public UrlLink Next { get; set; }

        public OverviewNavigationLinksModel(UrlLink previous, UrlLink next)
        {
            this.Previous = previous;
            this.Next = next;
        }
    }
}
