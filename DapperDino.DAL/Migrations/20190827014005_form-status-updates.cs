using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class formstatusupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormStatusUpdateStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatusUpdateStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormStatusUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    DiscordId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    FormType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatusUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormStatusUpdates_DiscordUsers_DiscordId",
                        column: x => x.DiscordId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormStatusUpdates_FormStatusUpdateStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "FormStatusUpdateStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormStatusUpdates_DiscordId",
                table: "FormStatusUpdates",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_FormStatusUpdates_StatusId",
                table: "FormStatusUpdates",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormStatusUpdates");

            migrationBuilder.DropTable(
                name: "FormStatusUpdateStatuses");
        }
    }
}
