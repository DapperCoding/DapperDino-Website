﻿using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.DAL
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed. 
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            builder.Entity<TicketReaction>()
                .HasOne(x => x.Ticket)
                .WithMany(x => x.Reactions)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<TicketUser>()
                .HasKey(bc => new { bc.TicketId, bc.DiscordUserId });
            
            builder.Entity<TicketUser>()
                .HasOne(bc => bc.Ticket)
                .WithMany(b => b.Assignees)
                .HasForeignKey(bc => bc.TicketId);

            builder.Entity<TicketUser>()
                .HasOne(bc => bc.DiscordUser)
                .WithMany(c => c.TicketUsers)
                .HasForeignKey(bc => bc.DiscordUserId);

        }

        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; }
        public DbSet<ResourceLink> ResourceLinks { get; set; }
        public DbSet<DiscordUser> DiscordUsers { get; set; }
        public DbSet<DiscordMessage> DiscordMessages { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketReaction> TicketReactions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }

}
