using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class GroceryItem : IPantryItem
    {
        [Key]
        public Guid GroceryItemId { get; set; }

        [Required]
        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Unit { get; set; }

        public DateTime? DateCompleted { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [NotMapped]
        public Plan[] Plans { get; set; }

        [NotMapped]
        public bool Purchased { get; set; }

        [NotMapped]
        public bool Deleted { get; set; }
    }
}
