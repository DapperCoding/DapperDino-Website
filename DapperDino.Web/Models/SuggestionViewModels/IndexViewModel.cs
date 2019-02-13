using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.SuggestionViewModels
{
    public class IndexViewModel
    {
        public List<SuggestionViewModel> Suggestions { get; set; }
    }
}
