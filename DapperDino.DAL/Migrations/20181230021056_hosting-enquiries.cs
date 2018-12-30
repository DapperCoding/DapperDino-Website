using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class hostingenquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_DiscordUsers_DiscordUserId",
                table: "Suggestions");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordUserId",
                table: "Suggestions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "DiscordMessages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HostingEnquiries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PackageType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostingEnquiries", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_DiscordUsers_DiscordUserId",
                table: "Suggestions",
                column: "DiscordUserId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_DiscordUsers_DiscordUserId",
                table: "Suggestions");

            migrationBuilder.DropTable(
                name: "HostingEnquiries");

            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "DiscordMessages");

            migrationBuilder.AlterColumn<int>(
                name: "DiscordUserId",
                table: "Suggestions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_DiscordUsers_DiscordUserId",
                table: "Suggestions",
                column: "DiscordUserId",
                principalTable: "DiscordUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
