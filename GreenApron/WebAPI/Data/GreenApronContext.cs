using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace WebAPI
{
    public class GreenApronContext : DbContext
    {
        public GreenApronContext(DbContextOptions<GreenApronContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<GroceryItem> GroceryItem { get; set; }
        public DbSet<InventoryItem> InventoryItem { get; set; }
        public DbSet<Bookmark> Bookmark { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<PlanIngredient> PlanIngredient { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Plan>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<GroceryItem>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<InventoryItem>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Bookmark>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<Ingredient>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<PlanIngredient>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
