using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class suggestionreactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuggestionReaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SuggestionId = table.Column<int>(nullable: false),
                    FromId = table.Column<int>(nullable: false),
                    DiscordMessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestionReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuggestionReaction_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuggestionReaction_DiscordUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuggestionReaction_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionReaction_DiscordMessageId",
                table: "SuggestionReaction",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionReaction_FromId",
                table: "SuggestionReaction",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestionReaction_SuggestionId",
                table: "SuggestionReaction",
                column: "SuggestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuggestionReaction");
        }
    }
}
