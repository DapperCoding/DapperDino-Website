using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class removedsomerelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedThumbnails_DiscordEmbedId",
                table: "DiscordEmbedThumbnails");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedVideos_DiscordEmbedId",
                table: "DiscordEmbedVideos");

            migrationBuilder.DropIndex(
               name: "IX_DiscordEmbedProviders_DiscordEmbedId",
               table: "DiscordEmbedProviders");

            migrationBuilder.DropIndex(
               name: "IX_DiscordEmbedImages_DiscordEmbedId",
               table: "DiscordEmbedImages");

            migrationBuilder.DropIndex(
              name: "IX_DiscordEmbedFooters_DiscordEmbedId",
              table: "DiscordEmbedFooters");

            migrationBuilder.DropIndex(
              name: "IX_DiscordEmbedAuthors_DiscordEmbedId",
              table: "DiscordEmbedAuthors");

            migrationBuilder.DropIndex(
              name: "IX_DiscordColors_DiscordEmbedId",
              table: "DiscordColors");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordColors_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordColors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "DiscordEmbeds");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "DiscordEmbeds");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedVideos_DiscordEmbedId",
                table: "DiscordEmbedVideos",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedThumbnails_DiscordEmbedId",
                table: "DiscordEmbedThumbnails",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedProviders_DiscordEmbedId",
                table: "DiscordEmbedProviders",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedImages_DiscordEmbedId",
                table: "DiscordEmbedImages",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedFooters_DiscordEmbedId",
                table: "DiscordEmbedFooters",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbedAuthors_DiscordEmbedId",
                table: "DiscordEmbedAuthors",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordColors_DiscordEmbedId",
                table: "DiscordColors",
                column: "DiscordEmbedId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordColors_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordColors",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedAuthors_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedAuthors");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedAuthors_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedAuthors",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedFooters_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedFooters");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedFooters_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedFooters",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedImages_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedImages");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedImages_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedImages",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedProviders_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedProviders");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedProviders_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedProviders",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedThumbnails_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedThumbnails");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedThumbnails_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedThumbnails",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(name: "FK_DiscordEmbedVideos_DiscordEmbeds_DiscordEmbedId", table: "DiscordEmbedVideos");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbedVideos_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedVideos",
                column: "DiscordEmbedId",
                principalTable: "DiscordEmbeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordColors_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordColors");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedAuthors_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedFooters_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedFooters");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedImages_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedImages");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedProviders_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedThumbnails_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedThumbnails");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscordEmbedVideos_DiscordEmbeds_DiscordEmbedId",
                table: "DiscordEmbedVideos");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedVideos_DiscordEmbedId",
                table: "DiscordEmbedVideos");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedThumbnails_DiscordEmbedId",
                table: "DiscordEmbedThumbnails");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedProviders_DiscordEmbedId",
                table: "DiscordEmbedProviders");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedImages_DiscordEmbedId",
                table: "DiscordEmbedImages");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedFooters_DiscordEmbedId",
                table: "DiscordEmbedFooters");

            migrationBuilder.DropIndex(
                name: "IX_DiscordEmbedAuthors_DiscordEmbedId",
                table: "DiscordEmbedAuthors");

            migrationBuilder.DropIndex(
                name: "IX_DiscordColors_DiscordEmbedId",
                table: "DiscordColors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FooterId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThumbnailId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "DiscordEmbeds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_AuthorId",
                table: "DiscordEmbeds",
                column: "AuthorId",
                unique: true,
                filter: "[AuthorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ColorId",
                table: "DiscordEmbeds",
                column: "ColorId",
                unique: true,
                filter: "[ColorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_FooterId",
                table: "DiscordEmbeds",
                column: "FooterId",
                unique: true,
                filter: "[FooterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ImageId",
                table: "DiscordEmbeds",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ProviderId",
                table: "DiscordEmbeds",
                column: "ProviderId",
                unique: true,
                filter: "[ProviderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_ThumbnailId",
                table: "DiscordEmbeds",
                column: "ThumbnailId",
                unique: true,
                filter: "[ThumbnailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordEmbeds_VideoId",
                table: "DiscordEmbeds",
                column: "VideoId",
                unique: true,
                filter: "[VideoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedAuthors_AuthorId",
                table: "DiscordEmbeds",
                column: "AuthorId",
                principalTable: "DiscordEmbedAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordColors_ColorId",
                table: "DiscordEmbeds",
                column: "ColorId",
                principalTable: "DiscordColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedFooters_FooterId",
                table: "DiscordEmbeds",
                column: "FooterId",
                principalTable: "DiscordEmbedFooters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedImages_ImageId",
                table: "DiscordEmbeds",
                column: "ImageId",
                principalTable: "DiscordEmbedImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedProviders_ProviderId",
                table: "DiscordEmbeds",
                column: "ProviderId",
                principalTable: "DiscordEmbedProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedThumbnails_ThumbnailId",
                table: "DiscordEmbeds",
                column: "ThumbnailId",
                principalTable: "DiscordEmbedThumbnails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordEmbeds_DiscordEmbedVideos_VideoId",
                table: "DiscordEmbeds",
                column: "VideoId",
                principalTable: "DiscordEmbedVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
