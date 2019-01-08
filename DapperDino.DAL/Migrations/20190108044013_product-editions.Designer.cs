﻿// <auto-generated />
using System;
using DapperDino.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DapperDino.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190108044013_product-editions")]
    partial class producteditions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DapperDino.DAL.Models.Applicant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<int>("DiscordUserId");

                    b.Property<string>("Explanation")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Links");

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId");

                    b.ToTable("Applicants");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<Guid>("DiscordRegistrationCode");

                    b.Property<int?>("DiscordUserId");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<bool>("RegisteredDiscordAccount");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.DiscordMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelId");

                    b.Property<int>("DiscordUserId");

                    b.Property<string>("GuildId");

                    b.Property<string>("ImageLink");

                    b.Property<bool>("IsDm");

                    b.Property<bool>("IsEmbed");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("MessageId")
                        .IsRequired();

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId");

                    b.ToTable("DiscordMessages");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.DiscordUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiscordId");

                    b.Property<int>("Level");

                    b.Property<string>("Name");

                    b.Property<string>("Username");

                    b.Property<int>("Xp");

                    b.HasKey("Id");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.FrequentlyAskedQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer");

                    b.Property<string>("Description");

                    b.Property<int?>("DiscordMessageId");

                    b.Property<string>("Question");

                    b.Property<int?>("ResourceLinkId");

                    b.HasKey("Id");

                    b.HasIndex("DiscordMessageId");

                    b.HasIndex("ResourceLinkId");

                    b.ToTable("FrequentlyAskedQuestions");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.HostingEnquiry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiscordId")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int>("PackageType");

                    b.HasKey("Id");

                    b.ToTable("HostingEnquiries");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("SendDate");

                    b.Property<int>("Status");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int?>("HeaderImageId");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.Property<double>("Price");

                    b.Property<int?>("ProductInstructionsId");

                    b.Property<double>("SalePercentage");

                    b.Property<string>("ShortDescription");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("ProductInstructionsId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductAmount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int?>("OrderId");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAmounts");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductEdition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ExtraInfo");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductEdition");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alt");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("ProductId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductInstructions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("ProductInstructions");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductProductCategory", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("ProductCategoryId");

                    b.HasKey("ProductId", "ProductCategoryId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("ProductProductCategory");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductProductEdition", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("ProductEditionId");

                    b.HasKey("ProductId", "ProductEditionId");

                    b.HasIndex("ProductEditionId");

                    b.ToTable("ProductProductEdition");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ResourceLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<string>("Link");

                    b.HasKey("Id");

                    b.ToTable("ResourceLinks");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Suggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int?>("DiscordUserId");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApplicantId");

                    b.Property<int>("Category");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastModified");

                    b.Property<int>("Priority");

                    b.Property<int>("Status");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.TicketReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DiscordMessageId");

                    b.Property<int>("FromId");

                    b.Property<int>("TicketId");

                    b.HasKey("Id");

                    b.HasIndex("DiscordMessageId");

                    b.HasIndex("FromId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketReactions");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.TicketUser", b =>
                {
                    b.Property<int>("TicketId");

                    b.Property<int>("DiscordUserId");

                    b.HasKey("TicketId", "DiscordUserId");

                    b.HasIndex("DiscordUserId");

                    b.ToTable("TicketUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Applicant", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany()
                        .HasForeignKey("DiscordUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ApplicationUser", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany()
                        .HasForeignKey("DiscordUserId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.DiscordMessage", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany()
                        .HasForeignKey("DiscordUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.FrequentlyAskedQuestion", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordMessage", "DiscordMessage")
                        .WithMany()
                        .HasForeignKey("DiscordMessageId");

                    b.HasOne("DapperDino.DAL.Models.ResourceLink", "ResourceLink")
                        .WithMany()
                        .HasForeignKey("ResourceLinkId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Order", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Product", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ProductImage", "HeaderImage")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("DapperDino.DAL.Models.ProductInstructions", "Instructions")
                        .WithMany()
                        .HasForeignKey("ProductInstructionsId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductAmount", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.Order")
                        .WithMany("ProductAmounts")
                        .HasForeignKey("OrderId");

                    b.HasOne("DapperDino.DAL.Models.ProductEdition", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductCategory", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ProductCategory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductEdition", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductImage", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductProductCategory", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ProductCategory", "ProductCategory")
                        .WithMany("Categories")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.Product", "Product")
                        .WithMany("Categories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.ProductProductEdition", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ProductEdition", "ProductEdition")
                        .WithMany("Group")
                        .HasForeignKey("ProductEditionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.Product", "Product")
                        .WithMany("Editions")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Suggestion", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany()
                        .HasForeignKey("DiscordUserId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.Ticket", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DapperDino.DAL.Models.TicketReaction", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordMessage", "DiscordMessage")
                        .WithMany()
                        .HasForeignKey("DiscordMessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.Ticket", "Ticket")
                        .WithMany("Reactions")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("DapperDino.DAL.Models.TicketUser", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.DiscordUser", "DiscordUser")
                        .WithMany("TicketUsers")
                        .HasForeignKey("DiscordUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.Ticket", "Ticket")
                        .WithMany("Assignees")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DapperDino.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DapperDino.DAL.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
