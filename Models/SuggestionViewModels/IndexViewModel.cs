using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.SuggestionViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<SuggestionViewModel> Suggestions { get; set; }
    }
}
