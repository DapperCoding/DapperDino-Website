using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class ticketlanguageframework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FrameworkId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FrameworkId",
                table: "Tickets",
                column: "FrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LanguageId",
                table: "Tickets",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Proficiencies_FrameworkId",
                table: "Tickets",
                column: "FrameworkId",
                principalTable: "Proficiencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Proficiencies_LanguageId",
                table: "Tickets",
                column: "LanguageId",
                principalTable: "Proficiencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Proficiencies_FrameworkId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Proficiencies_LanguageId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_FrameworkId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LanguageId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FrameworkId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Tickets");
        }
    }
}
