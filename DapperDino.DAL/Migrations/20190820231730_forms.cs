using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class forms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchitectForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<int>(nullable: false),
                    Motivation = table.Column<string>(nullable: true),
                    DevelopmentExperience = table.Column<string>(nullable: true),
                    PreviousIdeas = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchitectForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchitectForms_DiscordUsers_DiscordId",
                        column: x => x.DiscordId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruiterForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<int>(nullable: false),
                    Motivation = table.Column<string>(nullable: true),
                    DevelopmentExperience = table.Column<string>(nullable: true),
                    RecruitingExperience = table.Column<string>(nullable: true),
                    DevelopmentReviewingExperience = table.Column<string>(nullable: true),
                    GithubLink = table.Column<string>(nullable: true),
                    ProjectLinks = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruiterForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruiterForms_DiscordUsers_DiscordId",
                        column: x => x.DiscordId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<int>(nullable: false),
                    Motivation = table.Column<string>(nullable: true),
                    DevelopmentExperience = table.Column<string>(nullable: true),
                    GithubLink = table.Column<string>(nullable: true),
                    TeachingExperience = table.Column<string>(nullable: true),
                    ProjectLinks = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherForms_DiscordUsers_DiscordId",
                        column: x => x.DiscordId,
                        principalTable: "DiscordUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormReply<ArchitectForm>",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordMessageId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormReply<ArchitectForm>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormReply<ArchitectForm>_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormReply<ArchitectForm>_ArchitectForms_FormId",
                        column: x => x.FormId,
                        principalTable: "ArchitectForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormReply<RecruiterForm>",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordMessageId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormReply<RecruiterForm>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormReply<RecruiterForm>_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormReply<RecruiterForm>_RecruiterForms_FormId",
                        column: x => x.FormId,
                        principalTable: "RecruiterForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormReply<TeacherForm>",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscordMessageId = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormReply<TeacherForm>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormReply<TeacherForm>_DiscordMessages_DiscordMessageId",
                        column: x => x.DiscordMessageId,
                        principalTable: "DiscordMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormReply<TeacherForm>_TeacherForms_FormId",
                        column: x => x.FormId,
                        principalTable: "TeacherForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchitectForms_DiscordId",
                table: "ArchitectForms",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<ArchitectForm>_DiscordMessageId",
                table: "FormReply<ArchitectForm>",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<ArchitectForm>_FormId",
                table: "FormReply<ArchitectForm>",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<RecruiterForm>_DiscordMessageId",
                table: "FormReply<RecruiterForm>",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<RecruiterForm>_FormId",
                table: "FormReply<RecruiterForm>",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<TeacherForm>_DiscordMessageId",
                table: "FormReply<TeacherForm>",
                column: "DiscordMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FormReply<TeacherForm>_FormId",
                table: "FormReply<TeacherForm>",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruiterForms_DiscordId",
                table: "RecruiterForms",
                column: "DiscordId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherForms_DiscordId",
                table: "TeacherForms",
                column: "DiscordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormReply<ArchitectForm>");

            migrationBuilder.DropTable(
                name: "FormReply<RecruiterForm>");

            migrationBuilder.DropTable(
                name: "FormReply<TeacherForm>");

            migrationBuilder.DropTable(
                name: "ArchitectForms");

            migrationBuilder.DropTable(
                name: "RecruiterForms");

            migrationBuilder.DropTable(
                name: "TeacherForms");
        }
    }
}
