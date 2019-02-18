using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class requiredsuggestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Suggestions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Suggestions",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
