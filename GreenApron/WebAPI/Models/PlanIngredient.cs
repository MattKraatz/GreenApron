using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class PlanIngredient
    {
        [Key]
        public Guid PlanIngredientId { get; set; }

        [Required]
        [ForeignKey("Plan")]
        public Guid PlanId { get; set; }

        [Required]
        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string unit { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
    }
}
