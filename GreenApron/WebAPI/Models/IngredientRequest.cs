using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class IngredientRequest
    {
        public int id { get; set; }
        public string aisle { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public double amount { get; set; }
        public string unit { get; set; }
    }
}
