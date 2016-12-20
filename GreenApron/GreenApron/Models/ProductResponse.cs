using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class ProductResponse
    {
        public string type { get; set; }
        public Product[] products { get; set; }
        public int offset { get; set; }
        public int number { get; set; }
        public int totalProducts { get; set; }
        public int processingTimeMs { get; set; }
        public long expires { get; set; }
        public bool isStale { get; set; }
    }
}
