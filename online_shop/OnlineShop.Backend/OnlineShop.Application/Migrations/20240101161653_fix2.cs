using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Application.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorSchemaId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ProductSchemaId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductSchemaId",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSchemaId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_ProductSchemaId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorSchemaId",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ProductSchemaId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "ProductSchemaId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "ColorSchemaId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "ProductSchemaId",
                table: "ProductColors");

            migrationBuilder.RenameColumn(
                name: "SizeSchemaId",
                table: "ProductSizes",
                newName: "SizeSId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_SizeSchemaId",
                table: "ProductSizes",
                newName: "IX_ProductSizes_SizeSId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_ProductID",
                table: "ProductSizes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorID",
                table: "ProductColors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductID",
                table: "ProductColors",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorID",
                table: "ProductColors",
                column: "ColorID",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ProductID",
                table: "ProductColors",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductID",
                table: "ProductSizes",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSId",
                table: "ProductSizes",
                column: "SizeSId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Colors_ColorID",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ProductID",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductID",
                table: "ProductSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_ProductID",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorID",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ProductID",
                table: "ProductColors");

            migrationBuilder.RenameColumn(
                name: "SizeSId",
                table: "ProductSizes",
                newName: "SizeSchemaId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSizes_SizeSId",
                table: "ProductSizes",
                newName: "IX_ProductSizes_SizeSchemaId");

            migrationBuilder.AddColumn<int>(
                name: "ProductSchemaId",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorSchemaId",
                table: "ProductColors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductSchemaId",
                table: "ProductColors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_ProductSchemaId",
                table: "ProductSizes",
                column: "ProductSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorSchemaId",
                table: "ProductColors",
                column: "ColorSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductSchemaId",
                table: "ProductColors",
                column: "ProductSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Colors_ColorSchemaId",
                table: "ProductColors",
                column: "ColorSchemaId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ProductSchemaId",
                table: "ProductColors",
                column: "ProductSchemaId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductSchemaId",
                table: "ProductSizes",
                column: "ProductSchemaId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Sizes_SizeSchemaId",
                table: "ProductSizes",
                column: "SizeSchemaId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
