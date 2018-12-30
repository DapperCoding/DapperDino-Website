using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class HostingEnquiry:IEntity
    {
        [Required]
        public string DiscordId { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public HostingType PackageType { get; set; }
    }

    public enum HostingType
    {
        Small = 0,
        Pro = 1,
        Enterprise = 2
    }
}
