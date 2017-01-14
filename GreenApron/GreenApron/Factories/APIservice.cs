using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{

    /**
     * Purpose: Execute all calls to the Green Apron WebAPI
     *          Return an awaitable JsonResponse-base object
     * Methods:
     *     Task<JsonResponse> AddPlan(plan) - post a new plan
     *     Task<BookmarkResponse> AddBookmark() - post a new bookmark
     *     Task<BookmarkResponse> GetBookmarks() - get all bookmarks for this user
     *     Task<GroceryResponse> GetGroceryItems() - gets all grocery items for this user
     *     Task<JsonResponse> UpdateGroceryItems(request) - puts a provided list of grocery items
     *     Task<InventoryResponse> GetInventoryItems() - gets all inventory items for this user
     *     Task<PlanResponse> GetActivePlans() - gets all active meal plans for this user
     *     Task<JsonResponse> CompletePlan(planId) - puts a provided meal plan
     *     Task<JsonResponse> AddInventoryItem(item) - posts an inventory item
     *     Task<JsonResponse> AddGroceryItem(item) - posts a grocery item
     *     Task<JsonResponse> UpdateInventoryItems(request) - puts a provided list of inventory items
     *     Task<JsonResponse> DeleteInventoryItem(inventoryItemId) - deletes an inventory item
     *     Task<JsonResponse> DeleteGroceryItem(groceryItemId) - deletes a grocery item
     *     Task<BookmarkResponse> CheckBookmark(bookmark) - gets a provided bookmark, returns false if no bookmark exists
     *     Task<JsonResponse> DeleteBookmark(id) - deletes a bookmark
     *     Task<JsonResponse> DeletePlan(id) - deletes a plan
     */

    public class APIservice : IAPIservice
    {
        private HttpClient client;

        public APIservice()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        // POST a new plan
        public async Task<JsonResponse> AddPlan(PlanRequest plan)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/addplan", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(plan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // POST a new bookmark
        public async Task<BookmarkResponse> AddBookmark(BookmarkRequest bookmark)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(bookmark);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // GET a list of bookmarks for the logged-in user
        public async Task<BookmarkResponse> GetBookmarks()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // GET a list of grocery items for the logged-in user
        public async Task<GroceryResponse> GetGroceryItems()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GroceryResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // PUT a list of grocery items to be updated
        public async Task<JsonResponse> UpdateGroceryItems(GroceryRequest request)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/update", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // GET a list of inventory items for this user
        public async Task<InventoryResponse> GetInventoryItems()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<InventoryResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // GET a list of active plans for this user
        public async Task<PlanResponse> GetActivePlans()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PlanResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // PUT a provided plan to be updated
        public async Task<JsonResponse> CompletePlan(Guid planId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/complete/" + planId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // POST a new inventory item
        public async Task<JsonResponse> AddInventoryItem(Ingredient item)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/add/" + App.AuthManager.loggedInUser.UserId, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // POST a new grocery item
        public async Task<JsonResponse> AddGroceryItem(Ingredient item)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/add/" + App.AuthManager.loggedInUser.UserId, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // PUT a list of inventory items to be updated
        public async Task<JsonResponse> UpdateInventoryItems(InventoryRequest request)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/update", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // DELETE an inventory item
        public async Task<JsonResponse> DeleteInventoryItem(Guid inventoryItemId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/delete/" + inventoryItemId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // DELETE a grocery item
        public async Task<JsonResponse> DeleteGroceryItem(Guid groceryItemId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/delete/" + groceryItemId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // GET a specific bookmark, returns success = false if no matching bookmark was found
        public async Task<BookmarkResponse> CheckBookmark(int recipeId, Guid userId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/" + recipeId + "/" + userId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // DELETE a bookmark
        public async Task<JsonResponse> DeleteBookmark(Guid id)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/" + id, string.Empty));
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        // DELETE a plan
        public async Task<JsonResponse> DeletePlan(Guid id)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/delete/" + id, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
