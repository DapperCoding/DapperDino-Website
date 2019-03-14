using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DapperDino.DAL.Models
{
    // Add profile data for application users by adding properties to the AssignedTo class
    public class ApplicationUser : IdentityUser
    {
        public int? DiscordUserId { get; set; }
        [ForeignKey("DiscordUserId")]
        public virtual DiscordUser DiscordUser { get; set; }

        public Guid DiscordRegistrationCode { get; set; } = Guid.NewGuid();
        public bool RegisteredDiscordAccount { get; set; }
        public string WebsiteApiToken { get; set; }
    }
}
