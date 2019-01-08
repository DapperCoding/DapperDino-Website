using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DapperDino.DAL.Migrations
{
    public partial class producteditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_Products_ProductId",
                table: "ProductAmounts");

            migrationBuilder.CreateTable(
                name: "ProductEdition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExtraInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEdition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductEdition_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductEdition",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    ProductEditionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductEdition", x => new { x.ProductId, x.ProductEditionId });
                    table.ForeignKey(
                        name: "FK_ProductProductEdition_ProductEdition_ProductEditionId",
                        column: x => x.ProductEditionId,
                        principalTable: "ProductEdition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductProductEdition_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEdition_ProductId",
                table: "ProductEdition",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductEdition_ProductEditionId",
                table: "ProductProductEdition",
                column: "ProductEditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_ProductEdition_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "ProductEdition",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAmounts_ProductEdition_ProductId",
                table: "ProductAmounts");

            migrationBuilder.DropTable(
                name: "ProductProductEdition");

            migrationBuilder.DropTable(
                name: "ProductEdition");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAmounts_Products_ProductId",
                table: "ProductAmounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
