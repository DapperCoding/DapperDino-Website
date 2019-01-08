using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class producteditions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_ProductEdition_ProductId",
                table: "ProductAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductEdition_Products_ProductId",
                table: "ProductEdition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductEdition_ProductEdition_ProductEditionId",
                table: "ProductProductEdition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductEdition_Products_ProductId",
                table: "ProductProductEdition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductEdition",
                table: "ProductProductEdition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductEdition",
                table: "ProductEdition");

            migrationBuilder.RenameTable(
                name: "ProductProductEdition",
                newName: "ProductProductEditions");

            migrationBuilder.RenameTable(
                name: "ProductEdition",
                newName: "ProductEditions");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProductEdition_ProductEditionId",
                table: "ProductProductEditions",
                newName: "IX_ProductProductEditions_ProductEditionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEdition_ProductId",
                table: "ProductEditions",
                newName: "IX_ProductEditions_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductEditions",
                table: "ProductProductEditions",
                columns: new[] { "ProductId", "ProductEditionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductEditions",
                table: "ProductEditions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_ProductEditions_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "ProductEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEditions_Products_ProductId",
                table: "ProductEditions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductEditions_ProductEditions_ProductEditionId",
                table: "ProductProductEditions",
                column: "ProductEditionId",
                principalTable: "ProductEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductEditions_Products_ProductId",
                table: "ProductProductEditions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_ProductEditions_ProductId",
                table: "ProductAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductEditions_Products_ProductId",
                table: "ProductEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductEditions_ProductEditions_ProductEditionId",
                table: "ProductProductEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductEditions_Products_ProductId",
                table: "ProductProductEditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductEditions",
                table: "ProductProductEditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductEditions",
                table: "ProductEditions");

            migrationBuilder.RenameTable(
                name: "ProductProductEditions",
                newName: "ProductProductEdition");

            migrationBuilder.RenameTable(
                name: "ProductEditions",
                newName: "ProductEdition");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProductEditions_ProductEditionId",
                table: "ProductProductEdition",
                newName: "IX_ProductProductEdition_ProductEditionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEditions_ProductId",
                table: "ProductEdition",
                newName: "IX_ProductEdition_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductEdition",
                table: "ProductProductEdition",
                columns: new[] { "ProductId", "ProductEditionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductEdition",
                table: "ProductEdition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_ProductEdition_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "ProductEdition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEdition_Products_ProductId",
                table: "ProductEdition",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductEdition_ProductEdition_ProductEditionId",
                table: "ProductProductEdition",
                column: "ProductEditionId",
                principalTable: "ProductEdition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductEdition_Products_ProductId",
                table: "ProductProductEdition",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
