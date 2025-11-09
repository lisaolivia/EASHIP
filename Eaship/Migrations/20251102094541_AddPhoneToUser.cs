using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eaship.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "users",
                schema: "public",
                newName: "users",
                newSchema: "eaship");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "eaship",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "eaship",
                table: "users");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "eaship",
                newName: "users",
                newSchema: "public");
        }
    }
}
