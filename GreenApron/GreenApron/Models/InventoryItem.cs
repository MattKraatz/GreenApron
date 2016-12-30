using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class InventoryItem
    {
        public Guid InventoryItemId { get; set; }
        public int IngredientId { get; set; }
        public dbIngredient Ingredient { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public string AmountUnit { get; set; }
        public bool Empty { get; set; } = false;
        public int Count { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public Plan[] Plans { get; set; }
    }
}
