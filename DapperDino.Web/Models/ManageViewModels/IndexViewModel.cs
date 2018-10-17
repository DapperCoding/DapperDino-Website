using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        public string DiscordId { get; set; }
        public string DiscordUsername { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public bool IsDiscordConfirmed { get; set; }

        [Display(Name ="Discord Connect Code")]
        public Guid DiscordRegistrationCode { get; internal set; }
    }
}
