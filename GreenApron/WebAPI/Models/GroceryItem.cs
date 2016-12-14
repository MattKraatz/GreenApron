using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class GroceryItem
    {
        [Key]
        public Guid GroceryItemId { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Unit { get; set; }

        public string Aisle { get; set; }

        public string ImageURL { get; set; }

        public DateTime? DateCompleted { get; set; }

        [ForeignKey("Plan")]
        public Guid? PlanId { get; set; }
        public Plan Plan { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
    }
}
