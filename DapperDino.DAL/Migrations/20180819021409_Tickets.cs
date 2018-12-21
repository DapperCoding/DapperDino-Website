using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class Tickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    ApplicantId = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_DiscordUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tickets_DiscordUsers_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction, 
                        onUpdate:ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TicketReactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketId = table.Column<int>(nullable: false),
                    FromId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketReactions_DiscordUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketReactions_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketReactions_FromId",
                table: "TicketReactions",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketReactions_TicketId",
                table: "TicketReactions",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ApplicantId",
                table: "Tickets",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets",
                column: "AssignedToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketReactions");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
