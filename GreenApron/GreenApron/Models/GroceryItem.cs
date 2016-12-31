using System;

namespace GreenApron
{
    public class GroceryItem
    {
        public Guid GroceryItemId { get; set; }
        public int IngredientId { get; set; }
        public dbIngredient Ingredient { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public Plan[] Plans { get; set; }
        public int Count { get; set; }
        public DateTime? DateCompleted { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Purchased { get; set; }
        public bool Deleted { get; set; }
    }
}
