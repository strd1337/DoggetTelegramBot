using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModelWithFirstNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");
    }
}
