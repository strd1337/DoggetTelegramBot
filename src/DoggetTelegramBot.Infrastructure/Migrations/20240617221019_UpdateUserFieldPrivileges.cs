using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFieldPrivileges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Privilege",
                table: "Users");

            migrationBuilder.AddColumn<int[]>(
                name: "Privileges",
                table: "Users",
                type: "integer[]",
                nullable: false,
                defaultValue: Array.Empty<int>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Privileges",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Privilege",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
