using System;
using System.Collections.Generic;

namespace GreenApron
{
    public class dbIngredient
    {
        public int ingredientId { get; set; }
        public string ingredientName { get; set; }
        public string aisle { get; set; }
        public string imageURL { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
