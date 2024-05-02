using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Application.Migrations
{
    /// <inheritdoc />
    public partial class _ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFreeship",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFreeship",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
