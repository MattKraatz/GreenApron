using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class Plan
    {
        [Key]
        public Guid PlanId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Meal { get; set; }

        public int ServingsYield { get; set; }

        [Required]
        public string RecipeName { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public ICollection<PlanIngredient> PlanIngredients { get;set; }
    }
}
