using System;
using System.Collections.Generic;

namespace GreenApron
{
    public class Plan
    {
        public Guid PlanId { get; set; }
        public DateTime Date { get; set; }
        public string Meal { get; set; }
        public int ServingsYield { get; set; }
        public string RecipeName { get; set; }
        public int RecipeId { get; set; }
        public string RecipeImage { get; set; }
        public Recipe Recipe { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<PlanIngredient> PlanIngredients { get;set; }
    }
}
