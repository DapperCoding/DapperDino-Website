using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class applicationformstatusandtextstemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TeacherForms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RecruiterForms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ArchitectForms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TextTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Environment = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextTemplateKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TextId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextTemplateKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextTemplateKeys_TextTemplates_TextId",
                        column: x => x.TextId,
                        principalTable: "TextTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextTemplateKeys_TextId",
                table: "TextTemplateKeys",
                column: "TextId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextTemplateKeys");

            migrationBuilder.DropTable(
                name: "TextTemplates");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TeacherForms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RecruiterForms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ArchitectForms");
        }
    }
}
