using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models
{
    public class ApplyViewModel
    {
        [Required]
        [Display(Name = "Your discord username (ex: mickie456)")]
        public string DiscordId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name ="Your age")]
        public int Age { get; set; }

        [Display(Name = "Please tell us why you should be a Happy To Help member (min. 100 char)")]
        [Required, MinLength(100)]
        public string Explanation { get; set; }

        [Display(Name ="Links to code you've created (hastebin.com?)")]
        public string Links { get; set; }
    }
}
