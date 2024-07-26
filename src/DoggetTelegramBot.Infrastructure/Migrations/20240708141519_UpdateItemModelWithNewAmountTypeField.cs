using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemModelWithNewAmountTypeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
                name: "AmountType",
                table: "Items",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "AmountType",
                table: "Items");
    }
}
