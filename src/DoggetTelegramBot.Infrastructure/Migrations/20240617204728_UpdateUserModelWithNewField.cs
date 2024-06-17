using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModelWithNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
                name: "Privilege",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "Privilege",
                table: "Users");
    }
}
