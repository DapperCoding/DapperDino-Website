using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Applicant:IEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, MinLength(100)]
        public string Explanation { get; set; }

        [Required]
        public int Age { get; set; }

        public int DiscordUserId { get; set; }
        [ForeignKey("DiscordUserId")]
        public DiscordUser DiscordUser { get; set; }

        public string Links { get; set; }
    }
}
