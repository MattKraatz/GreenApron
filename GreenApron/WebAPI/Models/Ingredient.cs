using System.Collections.Generic;

namespace WebAPI
{
    public class Ingredient
    {
        public int id { get; set; }
        public string aisle { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public double amount { get; set; }
        public string unit { get; set; }
        public string unitShort { get; set; }
        public string unitLong { get; set; }
        public string originalString { get; set; }
        public List<object> metaInformation { get; set; }
    }
}
