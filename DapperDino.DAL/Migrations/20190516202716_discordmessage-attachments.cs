using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class discordmessageattachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DiscordEmbedProviders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "DiscordEmbedProviders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscordAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordAttachmentId = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    ProxyUrl = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    DiscordMessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordAttachment_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordAttachment_DiscordMessageId",
                table: "DiscordAttachment",
                column: "DiscordMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordAttachment");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DiscordEmbedProviders");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "DiscordEmbedProviders");
        }
    }
}
