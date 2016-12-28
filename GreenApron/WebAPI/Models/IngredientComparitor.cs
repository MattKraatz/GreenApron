using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class IngredientComparitor : IPantryItem
    {
        public int IngredientId { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set;}
        public double Count { get; set; }
        public string CountUnit { get; set; }
    }
}
