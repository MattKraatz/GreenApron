using System.Collections.Generic;

namespace GreenApron
{
    public class GroceryResponse : JsonResponse
    {
        public List<GroceryItem> GroceryItems { get; set; }
    }
}
