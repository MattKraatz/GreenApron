using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public interface IPantryItem
    {
        int IngredientId { get; set; }
        double Amount { get; set; }
        string Unit { get; set; }
    }
}
