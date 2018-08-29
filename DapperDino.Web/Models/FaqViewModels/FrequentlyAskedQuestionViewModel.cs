using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.FaqViewModels
{
    public class FrequentlyAskedQuestionViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public ResourceLinkViewModel ResourceLink { get; set; }
    }
}
