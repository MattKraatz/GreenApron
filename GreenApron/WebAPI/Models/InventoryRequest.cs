using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class InventoryRequest
    {
        public List<InventoryItem> items { get; set; }
    }
}
