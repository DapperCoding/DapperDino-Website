using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class ProductEnquiry : IEntity
    {
        [Required]
        public string DiscordId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public ProductEnquiryType PType { get; set; }
    }

    public enum ProductEnquiryType
    {
        ScrimBot = 0,
        MusicBot = 1
    }
}
