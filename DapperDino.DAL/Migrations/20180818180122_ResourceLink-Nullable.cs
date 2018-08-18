using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class ResourceLinkNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrequentlyAskedQuestions_ResourceLinks_ResourceLinkId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceLinkId",
                table: "FrequentlyAskedQuestions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentlyAskedQuestions_ResourceLinks_ResourceLinkId",
                table: "FrequentlyAskedQuestions",
                column: "ResourceLinkId",
                principalTable: "ResourceLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FrequentlyAskedQuestions_ResourceLinks_ResourceLinkId",
                table: "FrequentlyAskedQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceLinkId",
                table: "FrequentlyAskedQuestions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FrequentlyAskedQuestions_ResourceLinks_ResourceLinkId",
                table: "FrequentlyAskedQuestions",
                column: "ResourceLinkId",
                principalTable: "ResourceLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
