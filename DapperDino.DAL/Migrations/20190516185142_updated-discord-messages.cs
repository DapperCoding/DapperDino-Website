using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class updateddiscordmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DiscordUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBot",
                table: "DiscordUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DiscordEmbeds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordMessageId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: true),
                    ColorId = table.Column<int>(nullable: false),
                    FooterId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false),
                    ThumbnailId = table.Column<int>(nullable: false),
                    VideoId = table.Column<int>(nullable: false),
                    ProviderId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbeds_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordColors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordEmbedId = table.Column<int>(nullable: false),
                    G = table.Column<byte>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    R = table.Column<byte>(nullable: false),
                    B = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordColors_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IconUrl = table.Column<string>(nullable: true),
                    ProxyIconUrl = table.Column<string>(nullable: true),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedAuthors_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmbedId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Inline = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedFields_DiscordEmbeds_EmbedId",
                        column: x => x.EmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedFooters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    IconUrl = table.Column<string>(nullable: true),
                    ProxyIconUrl = table.Column<string>(nullable: true),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedFooters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedFooters_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    ProxyUrl = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedImages_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedProviders_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedThumbnails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    ProxyUrl = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedThumbnails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedThumbnails_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordEmbedVideos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    DiscordEmbedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordEmbedVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordEmbedVideos_DiscordEmbeds_DiscordEmbedId",
                        column: x => x.DiscordEmbedId,
                        principalTable: "DiscordEmbeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordColors_DiscordEmbedId",
                table: "DiscordColors",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedAuthors_DiscordEmbedId",
                table: "DiscordEmbedAuthors",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedFields_EmbedId",
                table: "DiscordEmbedFields",
                column: "EmbedId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedFooters_DiscordEmbedId",
                table: "DiscordEmbedFooters",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedImages_DiscordEmbedId",
                table: "DiscordEmbedImages",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedProviders_DiscordEmbedId",
                table: "DiscordEmbedProviders",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_DiscordMessageId",
                table: "DiscordEmbeds",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedThumbnails_DiscordEmbedId",
                table: "DiscordEmbedThumbnails",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedVideos_DiscordEmbedId",
                table: "DiscordEmbedVideos",
                column: "DiscordEmbedId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordColors");

            migrationBuilder.DropTable(
                name: "DiscordEmbedAuthors");

            migrationBuilder.DropTable(
                name: "DiscordEmbedFields");

            migrationBuilder.DropTable(
                name: "DiscordEmbedFooters");

            migrationBuilder.DropTable(
                name: "DiscordEmbedImages");

            migrationBuilder.DropTable(
                name: "DiscordEmbedProviders");

            migrationBuilder.DropTable(
                name: "DiscordEmbedThumbnails");

            migrationBuilder.DropTable(
                name: "DiscordEmbedVideos");

            migrationBuilder.DropTable(
                name: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DiscordUsers");

            migrationBuilder.DropColumn(
                name: "IsBot",
                table: "DiscordUsers");
        }
    }
}
