using System.Collections.Generic;

namespace WebAPI
{
    public class InventoryResponse : JsonResponse
    {
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
