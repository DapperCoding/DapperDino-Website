using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models
{
    public class ContactViewModel
    {
        [Required]
        [Display(Name ="Name*")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Discord id*")]
        public string DiscordId { get; set; }
        [Required]
        [Display(Name = "Discord username*")]
        public string DiscordUsername { get; set; }
        [Required]
        [Display(Name = "Your question*")]
        public string Question { get; set; }
    }
}
