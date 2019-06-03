using DapperDino.DAL.Models;
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

        public ApplicationDbContext():base()
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

            builder.Entity<SuggestionReaction>()
                .HasOne(x => x.Suggestion)
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



            builder.Entity<ProductProductCategory>()
                .HasKey(bc => new { bc.ProductId, bc.ProductCategoryId });

            builder.Entity<ProductProductCategory>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.Categories)
                .HasForeignKey(bc => bc.ProductId);

            builder.Entity<ProductProductCategory>()
                .HasOne(bc => bc.ProductCategory)
                .WithMany(c => c.Categories)
                .HasForeignKey(bc => bc.ProductCategoryId);


            builder.Entity<ProductProductCategory>()
               .HasKey(bc => new { bc.ProductId, bc.ProductCategoryId });

            builder.Entity<ProductProductCategory>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.Categories)
                .HasForeignKey(bc => bc.ProductId);

            builder.Entity<ProductProductCategory>()
                .HasOne(bc => bc.ProductCategory)
                .WithMany(c => c.Categories)
                .HasForeignKey(bc => bc.ProductCategoryId);


            builder.Entity<ProductProductEdition>()
               .HasKey(bc => new { bc.ProductId, bc.ProductEditionId });

            builder.Entity<ProductProductEdition>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.Editions)
                .HasForeignKey(bc => bc.ProductId);

            builder.Entity<ProductProductEdition>()
                .HasOne(bc => bc.ProductEdition)
                .WithMany(c => c.Group)
                .HasForeignKey(bc => bc.ProductEditionId);


            builder.Entity<DiscordEmbed>()
                .HasOne(x=>x.Author)
                .WithOne(x=>x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Color)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Author)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasMany(x => x.Fields)
                .WithOne(x => x.Embed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Footer)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Image)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Provider)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Thumbnail)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordEmbed>()
                .HasOne(x => x.Video)
                .WithOne(x => x.DiscordEmbed)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordMessage>()
                .HasMany(x => x.Embeds)
                .WithOne(x => x.DiscordMessage)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DiscordUserProficiency>()
                .HasOne(x => x.DiscordUser)
                .WithMany(x => x.Proficiencies);

            //builder.Entity<Ticket>()
            //    .HasOne(x => x.Framework)
            //    .WithMany(x => x.Tickets);

            //builder.Entity<Ticket>()
            //     .HasOne(x => x.Language)
            //     .WithMany(x => x.Tickets);
        }

        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; }
        public DbSet<ResourceLink> ResourceLinks { get; set; }
        public DbSet<DiscordUser> DiscordUsers { get; set; }
        public DbSet<DiscordMessage> DiscordMessages { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketReaction> TicketReactions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInstructions> ProductInstructions { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<HostingEnquiry> HostingEnquiries { get; set; }
        public DbSet<ProductEnquiry> ProductEnquiries { get; set; }
        public DbSet<ProductAmount> ProductAmounts { get; set; }
        public DbSet<ProductEdition> ProductEditions { get; set; }
        public DbSet<ProductProductEdition> ProductProductEditions { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<DiscordEmbed> DiscordEmbeds { get; set; }
        public DbSet<DiscordEmbedField> DiscordEmbedFields { get;set; }
        public DbSet<DiscordEmbedAuthor> DiscordEmbedAuthors { get; set; }
        public DbSet<DiscordEmbedProvider> DiscordEmbedProviders { get; set; }
        public DbSet<DiscordEmbedVideo> DiscordEmbedVideos { get; set; }
        public DbSet<DiscordEmbedThumbnail> DiscordEmbedThumbnails { get; set; }
        public DbSet<DiscordEmbedImage> DiscordEmbedImages { get; set; }
        public DbSet<DiscordEmbedFooter> DiscordEmbedFooters { get; set; }
        public DbSet<DiscordColor> DiscordColors { get; set; }


        public DbSet<Proficiency> Proficiencies { get; set; }
        public DbSet<DiscordUserProficiency> DiscordUserProficiencies { get; set; }
    }

}
