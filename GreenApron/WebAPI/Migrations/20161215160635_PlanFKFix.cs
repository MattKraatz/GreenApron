using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class PlanFKFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroceryItem_Plan_PlanId",
                table: "GroceryItem");

            migrationBuilder.DropIndex(
                name: "IX_GroceryItem_PlanId",
                table: "GroceryItem");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "GroceryItem");

            migrationBuilder.CreateIndex(
                name: "IX_PlanIngredient_PlanId",
                table: "PlanIngredient",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanIngredient_Plan_PlanId",
                table: "PlanIngredient",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanIngredient_Plan_PlanId",
                table: "PlanIngredient");

            migrationBuilder.DropIndex(
                name: "IX_PlanIngredient_PlanId",
                table: "PlanIngredient");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "GroceryItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItem_PlanId",
                table: "GroceryItem",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryItem_Plan_PlanId",
                table: "GroceryItem",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
