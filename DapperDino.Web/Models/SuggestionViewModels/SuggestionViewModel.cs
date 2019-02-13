using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDino.Models.SuggestionViewModels
{
    public class SuggestionViewModel:Suggestion
    {
        public string DiscordId { get; set; }
     
    }
}
