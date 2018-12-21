using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class initialdiscordmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DiscordMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<string>(nullable: false),
                    GuildId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ChannelId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    IsEmbed = table.Column<bool>(nullable: false),
                    IsDm = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordMessages");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tickets");
        }
    }
}
