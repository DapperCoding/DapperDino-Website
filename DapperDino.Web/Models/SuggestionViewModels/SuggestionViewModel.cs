using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDino.Models.SuggestionViewModels
{
    public class SuggestionViewModel:Suggestion
    {
        [Required]
        public string DiscordId { get; set; }
     
    }
}
