using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models
{
    public class ProductsViewModel
    {
        [Required]
        [Display(Name = "Your discord username (ex: mickie456#0295)")]
        public string DiscordId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        public string Product { get; set; }

        [Required]
        public ProductEnquiryType PType { get; set; }
    }
}
