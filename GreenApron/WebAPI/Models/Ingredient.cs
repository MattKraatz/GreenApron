using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class Ingredient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IngredientId { get; set; }

        [Required]
        public string IngredientName { get; set; }

        public string Aisle { get; set; }

        public string ImageURL { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public ICollection<GroceryItem> GroceryItems { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
        public ICollection<PlanIngredient> PlanIngredients { get; set; }

    }
}
