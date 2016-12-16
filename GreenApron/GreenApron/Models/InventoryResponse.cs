using System.Collections.Generic;

namespace GreenApron
{
    public class InventoryResponse : JsonResponse
    {
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
