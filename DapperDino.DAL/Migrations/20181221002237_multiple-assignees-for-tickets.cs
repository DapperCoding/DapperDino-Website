using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class multipleassigneesfortickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordUsers_Tickets_TicketId",
                table: "DiscordUsers");

            migrationBuilder.DropIndex(
                name: "IX_DiscordUsers_TicketId",
                table: "DiscordUsers");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "DiscordUsers");

            migrationBuilder.CreateTable(
                name: "TicketUser",
                columns: table => new
                {
                    TicketId = table.Column<int>(nullable: false),
                    DiscordUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketUser", x => new { x.TicketId, x.DiscordUserId });
                    table.ForeignKey(
                        name: "FK_TicketUser_DiscordUsers_DiscordUserId",
                        column: x => x.DiscordUserId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketUser_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketUser_DiscordUserId",
                table: "TicketUser",
                column: "DiscordUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketUser");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "DiscordUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUsers_TicketId",
                table: "DiscordUsers",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordUsers_Tickets_TicketId",
                table: "DiscordUsers",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
