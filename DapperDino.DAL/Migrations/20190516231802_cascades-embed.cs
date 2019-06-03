using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class cascadesembed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ThumbnailId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FooterId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ColorId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "DiscordEmbeds",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_AuthorId",
                table: "DiscordEmbeds",
                column: "AuthorId",
                unique: true,
                filter: "[AuthorId] IS NOT NULL AND [AuthorId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ColorId",
                table: "DiscordEmbeds",
                column: "ColorId",
                unique: true,
                filter: "[ColorId] IS NOT NULL AND [ColorId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_FooterId",
                table: "DiscordEmbeds",
                column: "FooterId",
                unique: true,
                filter: "[FooterId] IS NOT NULL AND [FooterId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ImageId",
                table: "DiscordEmbeds",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL AND [ImageId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ProviderId",
                table: "DiscordEmbeds",
                column: "ProviderId",
                unique: true,
                filter: "[ProviderId] IS NOT NULL AND [ProviderId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ThumbnailId",
                table: "DiscordEmbeds",
                column: "ThumbnailId",
                unique: true,
                filter: "[ThumbnailId] IS NOT NULL AND [ThumbnailId] <> 0");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_VideoId",
                table: "DiscordEmbeds",
                column: "VideoId",
                unique: true,
                filter: "[VideoId] IS NOT NULL AND [VideoId] <> 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_AuthorId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_ColorId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_FooterId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_ImageId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_ProviderId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_ThumbnailId",
                table: "DiscordEmbeds");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbeds_VideoId",
                table: "DiscordEmbeds");

            migrationBuilder.AlterColumn<int>(
                name: "VideoId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThumbnailId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FooterId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ColorId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "DiscordEmbeds",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_AuthorId",
                table: "DiscordEmbeds",
                column: "AuthorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ColorId",
                table: "DiscordEmbeds",
                column: "ColorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_FooterId",
                table: "DiscordEmbeds",
                column: "FooterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ImageId",
                table: "DiscordEmbeds",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ProviderId",
                table: "DiscordEmbeds",
                column: "ProviderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ThumbnailId",
                table: "DiscordEmbeds",
                column: "ThumbnailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_VideoId",
                table: "DiscordEmbeds",
                column: "VideoId",
                unique: true);
        }
    }
}
