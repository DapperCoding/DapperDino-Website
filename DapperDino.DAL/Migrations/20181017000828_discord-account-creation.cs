using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class discordaccountcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscordRegistrationCode",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "RegisteredDiscordAccount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordRegistrationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegisteredDiscordAccount",
                table: "AspNetUsers");
        }
    }
}
