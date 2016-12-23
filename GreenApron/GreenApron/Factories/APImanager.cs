using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class APImanager
    {
        IAPIservice _APIservice;

        public APImanager (IAPIservice service)
        {
            _APIservice = service;
        }

        public Task<JsonResponse> AddPlan(PlanRequest plan)
        {
            return _APIservice.AddPlan(plan);
        }

        public Task<JsonResponse> AddBookmark(BookmarkRequest bookmark)
        {
            return _APIservice.AddBookmark(bookmark);
        }

        public Task<BookmarkResponse> GetBookmarks()
        {
            return _APIservice.GetBookmarks();
        }

        public Task<GroceryResponse> GetGroceryItems()
        {
            return _APIservice.GetGroceryItems();
        }

        public Task<JsonResponse> UpdateGroceryItems(GroceryRequest request)
        {
            return _APIservice.UpdateGroceryItems(request);
        }

        public Task<InventoryResponse> GetInventoryItems()
        {
            return _APIservice.GetInventoryItems();
        }

        public Task<PlanResponse> GetActivePlans()
        {
            return _APIservice.GetActivePlans();
        }

        internal object AddGroceryItem(Ingredient item)
        {
            return _APIservice.AddGroceryItem(item);
        }

        internal object AddInventoryItem(Ingredient item)
        {
            return _APIservice.AddInventoryItem(item);
        }

        public Task<JsonResponse> CompletePlan(Guid id)
        {
            return _APIservice.CompletePlan(id);
        }
    }
}