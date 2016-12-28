using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenApron
{
    public class PlanIngredient
    {
        public Guid PlanIngredientId { get; set; }
        public Guid PlanId { get; set; }
        public int IngredientId { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
