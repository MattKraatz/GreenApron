using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public static class DisplayUnit
    {
        public static IngredientComparitor FromOunces(double ounces)
        {
            var output = new IngredientComparitor();
            if (ounces <= 0.5)
            {
                output.Amount = Math.Ceiling(ounces * 6);
                output.Unit = "tsp";
            }
            else if (ounces <= 1.5)
            {
                output.Amount = Math.Ceiling(ounces * 3);
                output.Unit = "tbs";
            } else if (ounces <= 64)
            {
                output.Amount = Math.Ceiling(ounces);
                output.Unit = "ounce";
            } else
            {
                output.Amount = Math.Round(ounces / 64,1);
                output.Unit = "gallon";
            }
            return output;
        }
    }
}
