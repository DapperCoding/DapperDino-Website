using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class ticketreactionaddmessageid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "TicketReactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "TicketReactions");
        }
    }
}
