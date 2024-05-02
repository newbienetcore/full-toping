using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Application.Migrations
{
    /// <inheritdoc />
    public partial class addshortdesc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeSId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "SizeSId",
                table: "ProductSizes");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeID",
                table: "ProductSizes",
                column: "SizeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeID",
                table: "ProductSizes",
                column: "SizeID",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeID",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_SizeID",
                table: "ProductSizes");

            migrationBuilder.AddColumn<int>(
                name: "SizeSId",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_SizeSId",
                table: "ProductSizes",
                column: "SizeSId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSId",
                table: "ProductSizes",
                column: "SizeSId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
