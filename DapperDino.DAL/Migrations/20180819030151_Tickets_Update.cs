using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class Tickets_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketReaction",
                table: "TicketReactions");

            migrationBuilder.RenameTable(
                name: "TicketReaction",
                newName: "TicketReactions");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReaction_TicketId",
                table: "TicketReactions",
                newName: "IX_TicketReactions_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReaction_FromId",
                table: "TicketReactions",
                newName: "IX_TicketReactions_FromId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketReactions",
                table: "TicketReactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions",
                column: "FromId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_DiscordUsers_FromId",
                table: "TicketReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReactions_Tickets_TicketId",
                table: "TicketReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketReactions",
                table: "TicketReactions");

            migrationBuilder.RenameTable(
                name: "TicketReactions",
                newName: "TicketReaction");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReactions_TicketId",
                table: "TicketReaction",
                newName: "IX_TicketReaction_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReactions_FromId",
                table: "TicketReaction",
                newName: "IX_TicketReaction_FromId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketReaction",
                table: "TicketReaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReaction_DiscordUsers_FromId",
                table: "TicketReaction",
                column: "FromId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReaction_Tickets_TicketId",
                table: "TicketReaction",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
