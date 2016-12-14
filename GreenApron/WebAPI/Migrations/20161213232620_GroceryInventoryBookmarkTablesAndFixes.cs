using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class GroceryInventoryBookmarkTablesAndFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "GroceryItem");

            migrationBuilder.CreateTable(
                name: "Bookmark",
                columns: table => new
                {
                    BookmarkId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ImageURL = table.Column<string>(nullable: true),
                    RecipeId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmark", x => x.BookmarkId);
                    table.ForeignKey(
                        name: "FK_Bookmark_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    InventoryItemId = table.Column<Guid>(nullable: false),
                    Aisle = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ImageURL = table.Column<string>(nullable: true),
                    IngredientName = table.Column<string>(nullable: false),
                    Unit = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.InventoryItemId);
                    table.ForeignKey(
                        name: "FK_InventoryItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "Aisle",
                table: "GroceryItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "GroceryItem",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCompleted",
                table: "GroceryItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_UserId",
                table: "Bookmark",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_UserId",
                table: "InventoryItem",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aisle",
                table: "GroceryItem");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "GroceryItem");

            migrationBuilder.DropTable(
                name: "Bookmark");

            migrationBuilder.DropTable(
                name: "InventoryItem");

            migrationBuilder.AddColumn<string>(
                name: "IngredientId",
                table: "GroceryItem",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCompleted",
                table: "GroceryItem",
                nullable: false);
        }
    }
}
