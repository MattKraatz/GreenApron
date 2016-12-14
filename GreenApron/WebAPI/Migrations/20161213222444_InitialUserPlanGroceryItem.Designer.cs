using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebAPI;

namespace WebAPI.Migrations
{
    [DbContext(typeof(GreenApronContext))]
    [Migration("20161213222444_InitialUserPlanGroceryItem")]
    partial class InitialUserPlanGroceryItem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebAPI.GroceryItem", b =>
                {
                    b.Property<Guid>("GroceryItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("DateCompleted");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("IngredientId")
                        .IsRequired();

                    b.Property<string>("IngredientName")
                        .IsRequired();

                    b.Property<Guid?>("PlanId");

                    b.Property<string>("Unit")
                        .IsRequired();

                    b.Property<Guid>("UserId");

                    b.HasKey("GroceryItemId");

                    b.HasIndex("PlanId");

                    b.HasIndex("UserId");

                    b.ToTable("GroceryItem");
                });

            modelBuilder.Entity("WebAPI.Plan", b =>
                {
                    b.Property<Guid>("PlanId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Meal")
                        .IsRequired();

                    b.Property<int>("RecipeId");

                    b.Property<string>("RecipeName")
                        .IsRequired();

                    b.Property<int>("ServingsYield");

                    b.Property<Guid>("UserId");

                    b.HasKey("PlanId");

                    b.HasIndex("UserId");

                    b.ToTable("Plan");
                });

            modelBuilder.Entity("WebAPI.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebAPI.GroceryItem", b =>
                {
                    b.HasOne("WebAPI.Plan", "Plan")
                        .WithMany("GroceryItems")
                        .HasForeignKey("PlanId");

                    b.HasOne("WebAPI.User", "User")
                        .WithMany("GroceryItems")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebAPI.Plan", b =>
                {
                    b.HasOne("WebAPI.User", "User")
                        .WithMany("Plans")
                        .HasForeignKey("UserId");
                });
        }
    }
}
