using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAPI.Migrations
{
    public partial class GroceryItemFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanIngredient_GroceryItem_GroceryItemId",
                table: "PlanIngredient");

            migrationBuilder.DropIndex(
                name: "IX_PlanIngredient_GroceryItemId",
                table: "PlanIngredient");

            migrationBuilder.DropColumn(
                name: "GroceryItemId",
                table: "PlanIngredient");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "Ingredient",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroceryItemId",
                table: "PlanIngredient",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanIngredient_GroceryItemId",
                table: "PlanIngredient",
                column: "GroceryItemId");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "Ingredient",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanIngredient_GroceryItem_GroceryItemId",
                table: "PlanIngredient",
                column: "GroceryItemId",
                principalTable: "GroceryItem",
                principalColumn: "GroceryItemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
