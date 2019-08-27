using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class custombotforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "FormStatusUpdates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomBotForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<int>(nullable: false),
                    Functionalities = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Budget = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomBotForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomBotForms_DiscordUsers_DiscordId",
                        column: x => x.DiscordId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomBotFormReply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordMessageId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomBotFormReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomBotFormReply_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomBotFormReply_CustomBotForms_FormId",
                        column: x => x.FormId,
                        principalTable: "CustomBotForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomBotFormReply_DiscordMessageId",
                table: "CustomBotFormReply",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomBotFormReply_FormId",
                table: "CustomBotFormReply",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomBotForms_DiscordId",
                table: "CustomBotForms",
                column: "DiscordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomBotFormReply");

            migrationBuilder.DropTable(
                name: "CustomBotForms");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "FormStatusUpdates");
        }
    }
}
