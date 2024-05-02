using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_RefreshToken_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_auth_refresh_token_OwnerId",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "Browser",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "auth_refresh_token");

            migrationBuilder.DropColumn(
                name: "UA",
                table: "auth_refresh_token");

            migrationBuilder.CreateIndex(
                name: "IX_auth_refresh_token_OwnerId",
                table: "auth_refresh_token",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_auth_refresh_token_OwnerId",
                table: "auth_refresh_token");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "auth_refresh_token",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "auth_refresh_token",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "auth_refresh_token",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "auth_refresh_token",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "auth_refresh_token",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UA",
                table: "auth_refresh_token",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_auth_refresh_token_OwnerId",
                table: "auth_refresh_token",
                column: "OwnerId",
                unique: true);
        }
    }
}
