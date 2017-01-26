using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class RecipePreview
    {
        public int id { get; set; }
        public string title { get; set; }
        public int readyInMinutes { get; set; }
        public string image { get; set; }
        public string[] imageUrls { get; set; }
    }
}
