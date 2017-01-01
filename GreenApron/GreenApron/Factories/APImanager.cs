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

        public Task<BookmarkResponse> AddBookmark(BookmarkRequest bookmark)
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

        public Task<JsonResponse> AddGroceryItem(Ingredient item)
        {
            return _APIservice.AddGroceryItem(item);
        }

        public Task<JsonResponse> UpdateInventoryItems(InventoryRequest request)
        {
            return _APIservice.UpdateInventoryItems(request);
        }

        public Task<JsonResponse> AddInventoryItem(Ingredient item)
        {
            return _APIservice.AddInventoryItem(item);
        }

        public Task<JsonResponse> CompletePlan(Guid id)
        {
            return _APIservice.CompletePlan(id);
        }

        public Task<JsonResponse> DeleteInventoryItem(Guid inventoryItemId)
        {
            return _APIservice.DeleteInventoryItem(inventoryItemId);
        }

        public Task<JsonResponse> DeleteGroceryItem(Guid groceryItemId)
        {
            return _APIservice.DeleteGroceryItem(groceryItemId);
        }

        public Task<BookmarkResponse> CheckBookmark(BookmarkRequest bookmark)
        {
            return _APIservice.CheckBookmark(bookmark);
        }

        public Task<JsonResponse> DeleteBookmark(Guid id)
        {
            return _APIservice.DeleteBookmark(id);
        }
    }
}