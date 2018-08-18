using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DapperDino.DAL.Models
{
    public class FrequentlyAskedQuestion
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int ResourceLinkId { get; set; }

        [ForeignKey(nameof(ResourceLinkId))]
        public virtual ResourceLink ResourceLink { get; set; }
    }
}
