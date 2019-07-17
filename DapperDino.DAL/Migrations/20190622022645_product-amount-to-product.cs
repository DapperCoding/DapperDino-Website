using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class productamounttoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_ProductEditions_ProductId",
                table: "ProductAmounts");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_Products_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_Products_ProductId",
                table: "ProductAmounts");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_ProductEditions_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "ProductEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
