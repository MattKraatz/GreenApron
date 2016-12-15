using System.Collections.Generic;

namespace WebAPI
{
    public class GroceryResponse : JsonResponse
    {
        public List<GroceryItem> GroceryItems { get; set; }
    }
}
