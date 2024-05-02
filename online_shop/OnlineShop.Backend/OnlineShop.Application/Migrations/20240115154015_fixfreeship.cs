using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Application.Migrations
{
    /// <inheritdoc />
    public partial class fixfreeship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersGroups_Customers_CustomerId",
                table: "CustomersGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomersGroups_Groups_GroupId",
                table: "CustomersGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerSchemaId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerSchemaId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersGroups",
                table: "CustomersGroups");

            migrationBuilder.DropColumn(
                name: "CustomerSchemaId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "CustomersGroups",
                newName: "CustomerGroups");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Orders",
                newName: "Total");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersGroups_GroupId",
                table: "CustomerGroups",
                newName: "IX_CustomerGroups_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersGroups_CustomerId",
                table: "CustomerGroups",
                newName: "IX_CustomerGroups_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "OrderSchemaId",
                table: "Vouchers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apartment",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CountryState",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "Subtotal",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TownCity",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerGroups",
                table: "CustomerGroups",
                column: "CustomerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_OrderSchemaId",
                table: "Vouchers",
                column: "OrderSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGroups_Customers_CustomerId",
                table: "CustomerGroups",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGroups_Groups_GroupId",
                table: "CustomerGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Orders_OrderSchemaId",
                table: "Vouchers",
                column: "OrderSchemaId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGroups_Customers_CustomerId",
                table: "CustomerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGroups_Groups_GroupId",
                table: "CustomerGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Orders_OrderSchemaId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_OrderSchemaId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerGroups",
                table: "CustomerGroups");

            migrationBuilder.DropColumn(
                name: "OrderSchemaId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Apartment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CountryState",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TownCity",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "CustomerGroups",
                newName: "CustomersGroups");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Orders",
                newName: "Price");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerGroups_GroupId",
                table: "CustomersGroups",
                newName: "IX_CustomersGroups_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerGroups_CustomerId",
                table: "CustomersGroups",
                newName: "IX_CustomersGroups_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerSchemaId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersGroups",
                table: "CustomersGroups",
                column: "CustomerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerSchemaId",
                table: "Orders",
                column: "CustomerSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersGroups_Customers_CustomerId",
                table: "CustomersGroups",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersGroups_Groups_GroupId",
                table: "CustomersGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerSchemaId",
                table: "Orders",
                column: "CustomerSchemaId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
