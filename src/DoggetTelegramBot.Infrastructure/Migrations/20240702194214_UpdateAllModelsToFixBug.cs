using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DoggetTelegramBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllModelsToFixBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyMembers",
                table: "FamilyMembers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transactions",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Marriages",
                newName: "MarriageId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Inventories",
                newName: "InventoryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Families",
                newName: "FamilyId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FamilyMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyMembers",
                table: "FamilyMembers",
                columns: ["Id", "FamilyId"]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyMembers",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FamilyMembers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MarriageId",
                table: "Marriages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Inventories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FamilyId",
                table: "Families",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyMembers",
                table: "FamilyMembers",
                columns: ["FamilyMemberId", "FamilyId"]);
        }
    }
}
